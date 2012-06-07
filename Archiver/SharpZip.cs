using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace T_Starter.Archiver {
	class SharpZip : IArchiver {
		public void Extract(FileInfo file) {
			Form1.Instance.PrepareExtraction();

			ZipFile zip = new ZipFile(file.FullName);
			long count = zip.Count;
			zip = null;
			long i = 0;

			using (ZipInputStream s = new ZipInputStream(file.OpenRead())) {
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(theEntry.Name);
					string fileName = Path.GetFileName(theEntry.Name);

					if (directoryName.Length > 0)
						Directory.CreateDirectory(file.DirectoryName + @"\temp\" + directoryName);

					if (fileName != String.Empty) {
						using (FileStream streamWriter = File.Create(file.DirectoryName + @"\temp\" + theEntry.Name)) {
							int size = 2048;
							byte[] data = new byte[2048];
							while (true) {
								size = s.Read(data, 0, data.Length);
								if (size > 0) streamWriter.Write(data, 0, size);
								else break;
								System.Windows.Forms.Application.DoEvents();
							}
						}
					}
					++i;
					Form1.Instance.SetPercentage((float)i / (float)count);
				}
			}

			Form1.Instance.SetPercentage(0);
			Form1.Instance.FinishExtraction();
		}
	}
}