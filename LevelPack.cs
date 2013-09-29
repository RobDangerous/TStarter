using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace T_Starter {
	abstract class LevelPack {
		protected string name, author, date;

		public override string ToString() {
			return name;
		}

		public string Name { get { return name; } }

		public string Author { get { return author; } }

		public string Date { get { return date; } }
		
		protected void Parse(string filename) {
			try {
				name = filename;
				foreach (var level in OnlineLevels.levels) {
					if (level.name == name + ".zip") {
						author = level.author;
						DateTime jan1970 = new DateTime(1970, 1, 2);
						DateTime time = jan1970.AddMilliseconds(double.Parse(level.date));
						date = time.ToShortDateString();
						break;
					}
				}
			}
			catch (Exception) {
				name = filename;
				author = "Unknown";
				date = "Unknown";
			}
		}

		protected static string ReplaceBars(string name) {
			string[] strings = name.Split('-', '_');
			name = "";
			foreach (string s in strings) {
				name += s.Substring(0, 1).ToUpper() + s.Substring(1) + " ";
			}
			name.Trim();
			return name;
		}

		public abstract Process Load();

		protected Process Start(DirectoryInfo dir) {
			if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 1) {
				return Start(dir.GetDirectories()[0]);
			}
			bool inifound = false;
			bool fexefound = false;
			RenameFiles(dir, ref inifound, ref fexefound);
			foreach(DirectoryInfo nextdir in dir.GetDirectories()) {
				if (nextdir.Name.ToLower() == "lvl") {
					CreateDirRenamer(nextdir.FullName, "Lvl");
					if (File.Exists(nextdir.FullName + "\\Level.ini")) inifound = true;
				}
				else if (nextdir.Name.ToLower() == "gfx") {
					CreateDirRenamer(nextdir.FullName, "GFX");
					if (Directory.Exists(nextdir.FullName + "\\Wasser"))
						CreateDirRenamer(nextdir.FullName + "\\Wasser", "GFX\\Wasser");
				}
				else if (nextdir.Name.ToLower() == "sfx") CreateDirRenamer(nextdir.FullName, "SFX");
				else if (nextdir.Name.ToLower() == "video") CreateDirRenamer(nextdir.FullName, "Video");
				else RenameFiles(nextdir, ref inifound, ref fexefound);
			}
			if (!inifound) {
				if (Directory.Exists(dir.FullName + "\\Lvl"))
					new Task.IniCreator(new DirectoryInfo(dir.FullName + "\\Lvl").GetFiles("*.lv6"));
				else new Task.IniCreator(dir.GetFiles("*.lv6"));
				CreateRenamer(@"Bonus\temp\Level.ini", @"Lvl\", "Level.ini");
			}
			Task.TaskManager.DoAll();
			Process t2002;
			if (fexefound) t2002 = Process.Start("T2002F.exe");
			else t2002 = Process.Start("T2002E.exe");
			return t2002;
		}

		private void RenameFiles(DirectoryInfo dir, ref bool inifound, ref bool fexefound) {
			foreach (FileInfo file in dir.GetFiles()) {
				if (file.Name.ToLower() == "level.ini") {
					inifound = true;
					CreateRenamer(file.FullName, "Lvl\\", "Level.ini");
				}
				else if (file.Name.ToLower() == "t2002f.exe") {
					fexefound = true;
					CreateRenamer(file.FullName, "", file.Name);
				}
				else if (file.Name.EndsWith(".bmp") || file.Name.EndsWith(".bl6") || file.Name.EndsWith(".lv6")) CreateRenamer(file.FullName, "Lvl\\", file.Name);
				else if (file.Name.EndsWith(".psm") || file.Name.EndsWith(".mp3")) CreateRenamer(file.FullName, "Music\\", file.Name);
			}
		}

		private void CreateRenamer(string from, string todir, string toname) {
			if (File.Exists(todir + toname)) CreateRenamer(todir + toname, todir, "_" + toname);
			new Task.Renamer(from, todir + toname);
		}

		private void CreateDirRenamer(string from, string to) {
			//if (Directory.Exists(to)) CreateDirRenamer(to, "_" + to);
			//new DirRenamer(from, to);
			DirectoryInfo dir = new DirectoryInfo(from);
			foreach (FileInfo file in dir.GetFiles()) CreateRenamer(file.FullName, to + "\\", file.Name);
		}
	}
}