using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class ArchiveExtractor : ITask {
		FileInfo archive;

		public ArchiveExtractor(FileInfo archive) {
			TaskManager.Add(this);
			this.archive = archive;
		}

		public void Do() {
			Archiver.Archiver.Extract(archive);
			Log.WriteLine("Extracted " + archive.Name + " to Bonus\\temp");
		}

		public void Undo() { }
	}
}