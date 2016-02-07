using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class IniFixer : ITask {
		public IniFixer() {
			TaskManager.Add(this);
		}

		public void Do() {
			try {
				string ini = File.ReadAllText(@"Lvl\level.ini");
				string[] lines = ini.Split('\n');
				for (int i = 0; i < lines.Length; ++i) {
					string line = lines[i];
					line = line.Trim();
					if (line.EndsWith(".psm")) {
						line = line.Substring(0, line.Length - 4) + ".mp3";
					}
					lines[i] = line;
				}

				File.WriteAllLines(@"Lvl\level.ini", lines);
				Log.WriteLine("Fixed level.ini");
			}
			catch (IOException) { }
		}

		public void Undo() {
			
		}
	}
}