using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace T_Starter.Archiver {
	public class SevenZip : IArchiver {
		string path;

		public SevenZip() {
			try {
				path = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("7-Zip").GetValue("Path").ToString();
			}
			catch (NullReferenceException) {
				System.Console.WriteLine("NullReferenceException");
			}
		}

		public void Extract(FileInfo file) {
			Process z = Process.Start(path + "\\7z.exe", "x \"" + file.FullName + "\" -o\"" + file.DirectoryName + "\\temp\\\"");
			z.WaitForExit();
		}
	}
}