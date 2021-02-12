using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CameraViewDesktop
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION
	{
		public byte BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public byte AlphaFormat;
	}

	public static class WinApi
	{
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DeleteObject(IntPtr hobject);

		public const byte AC_SRC_OVER = 0;
		public const byte AC_SRC_ALPHA = 1;
		public const int ULW_ALPHA = 2;

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int UpdateLayeredWindow(
			IntPtr hwnd,
			IntPtr hdcDst,
			[System.Runtime.InteropServices.In()]
			ref Point pptDst,
			[System.Runtime.InteropServices.In()]
			ref Size psize,
			IntPtr hdcSrc,
			[System.Runtime.InteropServices.In()]
			ref Point pptSrc,
			int crKey,
			[System.Runtime.InteropServices.In()]
			ref BLENDFUNCTION pblend,
			int dwFlags);
	}
}
