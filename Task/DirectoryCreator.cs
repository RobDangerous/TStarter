using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class DirectoryCreator : ITask {
		string dir;
		public DirectoryCreator(string dir) {
			TaskManager.Add(this);
			this.dir = dir;
		}

		public void Do() {
			try {
				Directory.CreateDirectory(dir);
				Log.WriteLine("Created directory \"" + dir + "\"");
			}
			catch (IOException) { }
		}

		public void Undo() {
			try {
				Directory.Delete(dir, true);
				Log.WriteLine("Deleted directory \"" + dir + "\"");
			}
			catch (IOException) { }
		}
	}
}