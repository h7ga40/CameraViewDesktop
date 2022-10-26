using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CameraViewDesktop.Properties;

namespace CameraViewDesktop
{
	public partial class MainForm : Form
	{
		private TestBench testBench;
		private CameraView cameraView;
		private Settings Settings = Settings.Default;
		private Point cameraViewLocation;
		private Size cameraViewSize;
		private int keyColorRange;

		public MainForm()
		{
			InitializeComponent();

			cameraViewLocation = Settings.CameraViewLocation;
			cameraViewSize = Settings.CameraViewSize;
			keyColorRange = Settings.KeyColorRange;

			cmbEffectorList.DisplayMember = "Name";
			cmbEffectorList.Items.Add(new NoEffector());
			cmbEffectorList.Items.Add(new RGBEffector());
			cmbEffectorList.Items.Add(new HSVEffector());
			cmbEffectorList.SelectedIndex = 0;

			var effector = Settings.Effector;
			foreach (var i in cmbEffectorList.Items) {
				if ((i as IEffector)?.Name == effector) {
					cmbEffectorList.SelectedItem = i;
					break;
				}
			}
			if (cmbEffectorList.Items.Count > 0 && cmbEffectorList.SelectedIndex == -1) {
				cmbEffectorList.SelectedIndex = 0;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Application.Idle += Application_Idle1;
		}

		private void Application_Idle1(object sender, EventArgs e)
		{
			Application.Idle -= Application_Idle1;

			testBench = new TestBench(this);
			testBench.UpdateDevices();

			var video = Settings.CameraName;
			var audio = Settings.MicName;

			cmbVideoList.Items.Clear();
			cmbVideoList.Items.AddRange(testBench.VideoDevices.ToArray());
			foreach (var i in cmbVideoList.Items) {
				if (i.ToString() == video) {
					cmbVideoList.SelectedItem = i;
					break;
				}
			}
			if (cmbVideoList.Items.Count > 0 && cmbVideoList.SelectedIndex == -1) {
				cmbVideoList.SelectedIndex = 0;
			}

			cmbAudioList.Items.Clear();
			cmbAudioList.Items.AddRange(testBench.AudioDevices.ToArray());
			foreach (var i in cmbAudioList.Items) {
				if (i.ToString() == video) {
					cmbAudioList.SelectedItem = i;
					break;
				}
			}
			if (cmbAudioList.Items.Count > 0 && cmbAudioList.SelectedIndex == -1) {
				cmbAudioList.SelectedIndex = 0;
			}

			string deviceName = Settings.Screen;

			if ((Screen.AllScreens.FirstOrDefault((screen) => screen.DeviceName == deviceName) == null)
				|| (Screen.AllScreens.FirstOrDefault((screen) => screen.Bounds.Contains(Settings.CameraViewLocation)) == null)) {
				Settings.Screen = deviceName;
				Settings.CameraViewLocation = new Point(0, 0);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (cameraView != null) {
				cameraView.Close();
				cameraView = null;

				timer1.Enabled = true;
			}
			else {
				var effector = (IEffector)cmbEffectorList.SelectedItem;
				var ef = effector as IKeyColor;
				if (ef != null) {
					ef.KeyColor = label1.BackColor;
					ef.KeyColorRange = trackBar1.Value;
				}

				var item = (Tuple<int, string, string>)cmbVideoList.SelectedItem;
				cameraView = new CameraView {
					Text = item == null ? "" : item.ToString(),
					TopMost = checkBox1.Checked,
					Effector = effector
				};

				if ((ModifierKeys & Keys.Control) == Keys.Control) {
					Settings.Screen = Screen.PrimaryScreen.DeviceName;
					Settings.CameraViewLocation = new Point(0, 0);
				}

				cameraView.SetVideoInput(item);
				cameraView.Show();
				cameraView.BringToFront();

				Settings.CameraName = cmbVideoList.SelectedItem.ToString();
				Settings.MicName = cmbAudioList.SelectedItem.ToString();
				Settings.Save();

				timer1.Enabled = true;
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (cameraView == null)
				return;

			cameraView.TopMost = checkBox1.Checked;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cameraView != null) {
				var effector = (IEffector)cmbEffectorList.SelectedItem;
				cameraView.Effector = effector;
				Settings.Effector = effector.Name;
				Settings.Save();
				var ef = effector as IKeyColor;
				if (ef != null) {
					ef.KeyColor = label1.BackColor;
					ef.KeyColorRange = trackBar1.Value;
				}
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{
			if (cameraView != null) {
				cameraView.GetPixelColor((Color color, bool done) => {
					label1.BackColor = color;
					if (done) {
						Settings.KeyColor = label1.BackColor;
						Settings.Save();
						var ef = cmbEffectorList.SelectedItem as IKeyColor;
						if (ef != null) {
							ef.KeyColor = color;
						}
					}
				});
			}
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			var ef = cmbEffectorList.SelectedItem as IKeyColor;
			if (ef != null) {
				ef.KeyColorRange = trackBar1.Value;
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (cameraView != null) {
				bool save = !Settings.IsSynchronized;
				var screen = Screen.FromControl(cameraView);
				if (Settings.Screen != screen.DeviceName) {
					save = true;
					Settings.Screen = screen.DeviceName;
				}
				if (cameraViewLocation != cameraView.Location) {
					save = true;
					cameraViewLocation = cameraView.Location;
				}
				if (cameraViewSize != cameraView.ClientSize) {
					save = true;
					cameraViewSize = cameraView.ClientSize;
				}
				if (keyColorRange != trackBar1.Value) {
					save = true;
					keyColorRange = trackBar1.Value;
				}
				if (save) {
					Settings.Save();
				}
			}
		}
	}
}
