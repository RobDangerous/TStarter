using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class DirRenamer : ITask {
		string from, to;

		public DirRenamer(string from, string to) {
			TaskManager.Add(this);
			this.from = from;
			this.to = to;
		}

		public void Do() {
			try {
				Directory.Move(from, to);
				Log.WriteLine("Moved directory \"" + from + "\" to \"" + to + "\"");
			}
			catch (IOException) { }
		}

		public void Undo() {
			try {
				Directory.Move(to, from);
				Log.WriteLine("Removed directory \"" + to + "\" to \"" + from + "\"");
			}
			catch (IOException) { }
		}
	}
}