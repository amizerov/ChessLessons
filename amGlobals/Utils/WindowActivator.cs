using System;
using System.Runtime.InteropServices;
using System.Text;

namespace am.Utils
{
	/// <summary>
	/// јктивирует окно уже запущенного экземпл€ра.
	/// </summary>
	public sealed class WindowActivator
	{
		private const string _user32Name = "user32.dll";
		private const int SW_SHOWNOACTIVATE = 4;

		[DllImport(_user32Name)]
		private static extern int SetForegroundWindow(IntPtr handle);

		[DllImport(_user32Name)]
		private static extern bool ShowWindow(IntPtr handle, int cmdShow);

		private delegate bool EnumWindowProc(IntPtr handle, int lParam);

		[DllImport(_user32Name)]
		private static extern bool EnumWindows(EnumWindowProc ewp, int lParam);

		[DllImport(_user32Name)]
		private static extern int GetWindowText(IntPtr handle, StringBuilder sb, int maxCount);

		private static string _wndName;

		public static void ActivateOldInstance(string wndName)
		{
			_wndName = wndName;
			try
			{
				EnumWindowProc ewp = new EnumWindowProc(WindowEnumerated);
				EnumWindows(ewp, 0);
				GC.KeepAlive(ewp);
			}
			finally
			{
				_wndName = null;
			}
		}

		private static bool WindowEnumerated(IntPtr handle, int lParam)
		{
			const int bufSize = 1000;
			StringBuilder sb = new StringBuilder(bufSize);
			GetWindowText(handle, sb, bufSize - 1);
			if (sb.ToString() == _wndName)
			{
				SetForegroundWindow(handle);
				ShowWindow(handle, SW_SHOWNOACTIVATE);
				return false;
			}
			return true;
		}
	}
}