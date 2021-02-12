using System;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace CameraViewDesktop
{
	internal interface IKeyColor
	{
		Color KeyColor { get; set; }
		int KeyColorRange { get; set; }
	}

	internal class NoEffector : IEffector
	{
		public string Name => "効果なし";

		public Bitmap ToBitmap(Mat normalFrame)
		{
			return BitmapConverter.ToBitmap(normalFrame);
		}
	}

	internal class RGBEffector : IEffector, IKeyColor
	{
		public string Name => "透過（RGB）";

		public Color KeyColor { get; set; }

		public int KeyColorRange { get; set; } = 10;

		public Bitmap ToBitmap(Mat normalFrame)
		{
			Bitmap bitmap;

			var rgb = Cv2.Split(normalFrame);
			var grayFrame = new Mat();
			var rgba = new Mat();
			//Cv2.CvtColor(normalFrame, grayFrame, ColorConversionCodes.BGR2GRAY);
			var s_min = new Scalar(
				Math.Max(0, KeyColor.B - KeyColorRange),
				Math.Max(0, KeyColor.G - KeyColorRange),
				Math.Max(0, KeyColor.R - KeyColorRange));
			var s_max = new Scalar(
				Math.Min(255, KeyColor.B + KeyColorRange),
				Math.Min(255, KeyColor.G + KeyColorRange),
				Math.Min(255, KeyColor.R + KeyColorRange));
			Cv2.InRange(normalFrame, s_min, s_max, grayFrame);
			Cv2.BitwiseNot(grayFrame, grayFrame);
			var rgbaLayers = new Mat[] { rgb[0], rgb[1], rgb[2], grayFrame };
			Cv2.Merge(rgbaLayers, rgba);
			bitmap = BitmapConverter.ToBitmap(rgba);

			grayFrame.Dispose();
			rgba.Dispose();

			return bitmap;
		}
	}

	internal class HSVEffector : IEffector, IKeyColor
	{
		public string Name => "透過（HSV）";

		public Color KeyColor { get; set; }

		public int KeyColorRange { get; set; } = 10;

		public Bitmap ToBitmap(Mat normalFrame)
		{
			Bitmap bitmap;

			ConvertBGR2HSV(KeyColor.B, KeyColor.G, KeyColor.R, out var h, out var s, out var v);

			var hsv = new Mat();
			Cv2.CvtColor(normalFrame, hsv, ColorConversionCodes.BGR2HSV);
			var grayFrame = new Mat();
			var s_min = new Scalar(
				Math.Max(0, h - 10),
				Math.Max(0, s - KeyColorRange),
				Math.Max(0, v - KeyColorRange));
			var s_max = new Scalar(
				Math.Min(255, h + 10),
				Math.Min(255, s + KeyColorRange),
				Math.Min(255, v + KeyColorRange));
			Cv2.InRange(hsv, s_min, s_max, grayFrame);
			Cv2.BitwiseNot(grayFrame, grayFrame);

			var rgb = Cv2.Split(normalFrame);
			var rgbaLayers = new Mat[] { rgb[0], rgb[1], rgb[2], grayFrame };
			var rgba = new Mat();
			Cv2.Merge(rgbaLayers, rgba);
			bitmap = BitmapConverter.ToBitmap(rgba);

			hsv.Dispose();
			grayFrame.Dispose();
			rgba.Dispose();

			return bitmap;
		}

		private void ConvertBGR2HSV(byte b, byte g, byte r, out ushort h, out byte s, out byte v)
		{
			byte max, min;
			if ((r > b) && (r > g)) {
				max = r; min = Math.Min(b, g);
				h = (byte)((30 * (g - b)) / (max - min));
			}
			else if ((g > r) && (g > b)) {
				max = g; min = Math.Min(r, b);
				h = (byte)((30 * (b - r)) / (max - min) + 60);
			}
			else {
				max = b; min = Math.Min(g, r);
				h = (byte)((30 * (r - g)) / (max - min) + 120);
			}
			s = (byte)(255 * (max - min) / max);
			v = max;
		}
	}
}
