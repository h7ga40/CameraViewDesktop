using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace CameraViewDesktop
{
	internal interface IEffector
	{
		string Name { get; }
		Bitmap ToBitmap(Mat normalFrame);
	}

	public partial class CameraView : Form
	{
		private int deviceId;
		private Task thread;
		private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();
		private string error;
		private Bitmap bitmap;
		internal IEffector Effector { get; set; }

		public CameraView()
		{
			bitmap = new Bitmap(Width, Height);
			InitializeComponent();
			originalBounds = Bounds;
		}

		protected override CreateParams CreateParams {
			get {
				const int WS_EX_LAYERED = 0x00080000;

				var cp = base.CreateParams;
				cp.ExStyle = cp.ExStyle | WS_EX_LAYERED;

				return cp;
			}
		}

		private static BLENDFUNCTION blend = new BLENDFUNCTION {
			BlendOp = WinApi.AC_SRC_OVER,
			BlendFlags = 0,
			SourceConstantAlpha = 255,
			AlphaFormat = WinApi.AC_SRC_ALPHA
		};

		private void CameraView_Load(object sender, EventArgs e)
		{
			UpdateImage();
		}

		private void CameraView_Resize(object sender, EventArgs e)
		{
			UpdateImage();
		}

		private void UpdateImage()
		{
			var srcPos = new System.Drawing.Point(0, 0);
			var srcSize = new System.Drawing.Size(bitmap.Width, bitmap.Height);
			var dstPos = new System.Drawing.Point(Left, Top);
			var dstSize = new System.Drawing.Size(Width, Height);
			Rectangle rectangle;

			var (x, y) = (srcSize.Width / (float)dstSize.Width, srcSize.Height / (float)dstSize.Height);
			if (x > y) {
				var size = new System.Drawing.Size(dstSize.Width, (int)(srcSize.Height / x));
				rectangle = new Rectangle(0, (dstSize.Height - size.Height) / 2, size.Width, size.Height);
			}
			else {
				var size = new System.Drawing.Size((int)(srcSize.Width / y), dstSize.Height);
				rectangle = new Rectangle((dstSize.Width - size.Width) / 2, 0, size.Width, size.Height);
			}

			var dstBitmap = new Bitmap(dstSize.Width, dstSize.Height);
			using (var image = Graphics.FromImage(dstBitmap)) {
				image.DrawImage(bitmap, rectangle);

				if (activated) {
					image.DrawRectangle(Pens.Silver, new Rectangle(0, 0, dstSize.Width - 1, dstSize.Height - 1));
					image.DrawRectangle(Pens.Silver, new Rectangle(1, 1, dstSize.Width - 3, dstSize.Height - 3));
					image.DrawRectangle(Pens.Silver, new Rectangle(2, 2, dstSize.Width - 5, dstSize.Height - 5));
					image.FillRectangle(Brushes.Silver, new Rectangle(3, 3, dstSize.Width - 6, 8));
				}

				using (var screen = Graphics.FromHwnd(IntPtr.Zero)) {
					var hdc_screen = screen.GetHdc();
					var hdc_image = image.GetHdc();
					var oldhbmp = WinApi.SelectObject(hdc_image, dstBitmap.GetHbitmap(Color.FromArgb(0)));

					try {
						WinApi.UpdateLayeredWindow(
							Handle, hdc_screen, ref dstPos, ref dstSize,
							hdc_image, ref srcPos, 0, ref blend, WinApi.ULW_ALPHA);
					}
					finally {
						WinApi.DeleteObject(WinApi.SelectObject(hdc_image, oldhbmp));
						screen.ReleaseHdc(hdc_screen);
						image.ReleaseHdc(hdc_image);
					}
				}
			}
			dstBitmap.Dispose();
		}

		private bool spoitMode = false;
		private Action<Color, bool> GotPixelColor;

		internal void GetPixelColor(Action<Color, bool> callback)
		{
			spoitMode = true;
			Cursor = Cursors.Cross;
			GotPixelColor = callback;
		}

		private async Task captureThread()
		{
			var count = 0;
			try {
				var capture = new VideoCapture(deviceId);
				while (!cancelToken.IsCancellationRequested) {
					if (capture.IsOpened()) {
						Bitmap bitmap = null;
						var normalFrame = new Mat();
						capture.Read(normalFrame);

						try {
							bitmap = Effector?.ToBitmap(normalFrame);
						}
						catch (Exception e) {
							error = e.Message;
						}

						if (bitmap == null)
							bitmap = BitmapConverter.ToBitmap(normalFrame);
						normalFrame.Dispose();

						if (count == 0) {
							count++;
							BeginInvoke(new MethodInvoker(() => {
								if (this.bitmap != null) {
									this.bitmap.Dispose();
								}

								this.bitmap = bitmap;
								UpdateImage();
								count--;
							}));
						}
					}

					await Task.Delay(1);
				}

				capture.Release();
			}
			catch (Exception e) {
				error = e.Message;
			}
		}

		internal void SetVideoInput(Tuple<int, string, string> item)
		{
			if (item == null)
				return;

			deviceId = item.Item1;

			if (thread != null) {
				cancelToken.Cancel();
				try {
					thread.Wait();
				}
				catch (Exception) {

				}
			}

			thread = new Task(async () => { await captureThread(); });
			thread.Start();
		}

		private void CameraView_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (thread != null) {
				cancelToken.Cancel();
				try {
					thread.Wait();
				}
				catch (Exception) {

				}
				thread = null;
			}
		}

		private Rectangle originalBounds;
		private bool maximaized;

		public void ShowMax()
		{
			if (!maximaized) {
				var screen = Screen.FromControl(this);
				//Bounds = screen.Bounds;
				WindowState = FormWindowState.Maximized;
				FormBorderStyle = FormBorderStyle.None;
				maximaized = true;
			}
		}

		private void CameraView_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
			case Keys.Space:
				originalBounds = Bounds;
				ShowMax();
				break;
			case Keys.Escape:
				WindowState = FormWindowState.Normal;
				FormBorderStyle = FormBorderStyle.Sizable;
				Bounds = originalBounds;
				maximaized = false;
				break;
			default:
				break;
			}
		}

		private bool down;
		private System.Drawing.Point start = new System.Drawing.Point();

		private void CameraView_MouseDown(object sender, MouseEventArgs e)
		{
			down = true;
			start = e.Location;
		}

		private void CameraView_MouseUp(object sender, MouseEventArgs e)
		{
			down = false;

			if (spoitMode) {
				spoitMode = false;
				Cursor = Cursors.Arrow;
				var color = GetPixel(e.X, e.Y);
				GotPixelColor?.Invoke(color, true);
			}
		}

		private void CameraView_MouseMove(object sender, MouseEventArgs e)
		{
			if (down) {
				//var current = PointToScreen(e.Location);
				var current = new System.Drawing.Point(e.Location.X + Left, e.Location.Y + Top);
				var x = current.X - start.X;
				var y = current.Y - start.Y;
				Location = new System.Drawing.Point(x, y);
			}

			if (spoitMode) {
				var color = GetPixel(e.X, e.Y);
				GotPixelColor?.Invoke(color, false);
			}
		}

		private Color GetPixel(int x, int y)
		{
			var ax = bitmap.Width / (double)ClientSize.Width;
			var ay = bitmap.Height / (double)ClientSize.Height;
			var a = Math.Min(ax, ay);
			var color = bitmap.GetPixel((int)(a * x) - ClientRectangle.Left, (int)(a * y) - ClientRectangle.Top);
			return color;
		}

		private void CameraView_DoubleClick(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
				WindowState = FormWindowState.Maximized;
			else
				WindowState = FormWindowState.Normal;
		}

		private bool activated;

		private void CameraView_Activated(object sender, EventArgs e)
		{
			activated = true;
			UpdateImage();
		}

		private void CameraView_Deactivate(object sender, EventArgs e)
		{
			activated = false;
			UpdateImage();
		}
	}
}
