using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace CameraViewDesktop
{
	[InterfaceType(1)]
	[Guid("3127CA40-446E-11CE-8135-00AA004BB851")]
	public interface IErrorLog
	{
		void AddError(string pszPropName, ref System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo);
	}

	[ComImport, Guid("29840822-5b84-11d0-bd3b-00a0c911ce86")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface ICreateDevEnum
	{
		void CreateClassEnumerator(
			ref Guid clsidDeviceClass,
			[Out]
			out IEnumMoniker ppEnumMoniker,
			int dwFlags /*reserved must be 0*/
		);
	}

	[ComImport, Guid("55272a00-42cb-11ce-8135-00aa004bb851")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyBag
	{
		[PreserveSig]
		int Read(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[Out, MarshalAs(UnmanagedType.Struct)] out object pVar,
			[In] IErrorLog pErrorLog
		);

		[PreserveSig]
		int Write(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In, MarshalAs(UnmanagedType.Struct)] ref object pVar
		);
	}

	enum DeviceType
	{
		Video,
		Audio
	}

	class TestBench
	{
		private MainForm mainForm;

		public List<Tuple<int, string, string>> VideoDevices { get; } = new List<Tuple<int, string, string>>();
		public List<Tuple<int, string, string>> AudioDevices { get; } = new List<Tuple<int, string, string>>();

		public TestBench(MainForm mainForm)
		{
			this.mainForm = mainForm;
		}

		internal void UpdateDevices()
		{
			var tDevEnum = Type.GetTypeFromCLSID(
				new Guid("62be5d10-60eb-11d0-bd3b-00a0c911ce86"));
			var devEnum = (ICreateDevEnum)Activator.CreateInstance(tDevEnum);
			IEnumMoniker enumMoniker = null;
			var cat = new Guid("860bb310-5d01-11d0-bd3b-00a0c911ce86");

			VideoDevices.Clear();
			AudioDevices.Clear();

			devEnum.CreateClassEnumerator(ref cat, out enumMoniker, 0);
			if (enumMoniker != null) {
				VideoDevices.AddRange(DisplayDeviceInformation(DeviceType.Video, enumMoniker));
			}

			cat = new Guid("33d9a762-90c8-11d0-bd43-00a0c911ce86");
			devEnum.CreateClassEnumerator(ref cat, out enumMoniker, 0);
			if (enumMoniker != null) {
				AudioDevices.AddRange(DisplayDeviceInformation(DeviceType.Audio, enumMoniker));
			}
		}

		private List<Tuple<int, string, string>> DisplayDeviceInformation(DeviceType video, IEnumMoniker enumMoniker)
		{
			var list = new List<Tuple<int, string, string>>();
			IMoniker[] pMoniker = new IMoniker[1];
			int index = -1;

			while (enumMoniker.Next(1, pMoniker, IntPtr.Zero) == 0) {
				index++;
				var guid = new Guid("55272A00-42CB-11CE-8135-00AA004BB851");
				pMoniker[0].BindToStorage(null, null, guid, out var pPropBag);
				var propBag = (IPropertyBag)pPropBag;
				object variant;

				propBag.Read("Description", out variant, null);
				string description = variant == null ? "" : variant.ToString();
				propBag.Read("FriendlyName", out variant, null);
				string friendlyName = variant == null ? "" : variant.ToString();

				list.Add(new Tuple<int, string, string>(index, description, friendlyName));
			}

			return list;
		}
	}
}
