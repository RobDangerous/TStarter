using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace T_Starter.Archiver {
	public class WinRAR : IArchiver {
		string exe;

		public WinRAR() {
			exe = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Applications").OpenSubKey("WinRAR.exe").OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue("").ToString();
			exe = exe.Substring(1, exe.Length - 7);
		}

		public void Extract(FileInfo file) {
			Process z = Process.Start(exe, "x \"" + file.FullName + "\" \"" + file.DirectoryName + "\\temp\\\"");
			z.WaitForExit();
		}
	}
}