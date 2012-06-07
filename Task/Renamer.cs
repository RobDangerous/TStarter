using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class Renamer : ITask {
		string from, to;

		public Renamer(string from, string to) {
			TaskManager.Add(this);
			this.from = from;
			this.to = to;
		}

		public void Do() {
			try {
				File.Move(from, to);
				Log.WriteLine("Moved file \"" + from + "\" to \"" + to + "\"");
			}
			catch (IOException) { }
		}

		public void Undo() {
			try {
				File.Move(to, from);
				Log.WriteLine("Removed file \"" + to + "\" to \"" + from + "\"");
			}
			catch (IOException) { }
		}
	}
}