using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Archiver {
	public class Archiver {
		static IArchiver archiver;

		static Archiver() {
			archiver = new SharpZip();

			/*try {
				archiver = new MiniSevenZip();
				return;
			}
			catch (Exception) { }
			try {
				archiver = new SevenZip();
				return;
			}
			catch (NullReferenceException) { }
			try {
				archiver = new WinRAR();
				return;
			}
			catch (NullReferenceException) { }*/
		}

		public static void Extract(FileInfo file) {
			try {
				archiver.Extract(file);
			}
			catch (Exception) {
				throw new ArchiveException();
			}
		}
	}
}