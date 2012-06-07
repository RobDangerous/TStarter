using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace T_Starter.Archiver {
	public class MiniSevenZip : IArchiver {
		public MiniSevenZip() {
			FileInfo file = new FileInfo("7-Zip\\7z.exe");
			if (!file.Exists) throw new Exception();
		}

		public void Extract(FileInfo file) {
			Process z = Process.Start("7-Zip\\7z.exe", "x \"" + file.FullName + "\" -o\"" + file.DirectoryName + "\\temp\\\"");
			z.WaitForExit();
		}
	}
}