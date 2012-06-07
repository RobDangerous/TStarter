using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Task {
	[Serializable()]
	public class IniCreator : ITask {
		FileInfo[] levels;

		public IniCreator(FileInfo[] levels) {
			TaskManager.Add(this);
			this.levels = levels;
		}

		public void Do() {
			try {
				StreamWriter ini = File.CreateText("Bonus\\temp\\level.ini");
				ini.WriteLine("[All]");
				ini.WriteLine("Count=" + levels.Length);
				ini.WriteLine();
				ini.WriteLine("[Level0]");
				ini.WriteLine("Name=Title.lv6");
				ini.WriteLine("Back=Bg_black.bmp");
				ini.WriteLine("Music=Title.psm");
				ini.WriteLine("Tfmx=T20.tfx");
				ini.WriteLine("Sub=0");
				for (int i = 0; i < levels.Length; ++i) {
					FileInfo level = (FileInfo)levels[i];
					ini.WriteLine();
					ini.WriteLine("[Level" + (i + 1) + "]");
					ini.WriteLine("Name=" + level.Name);
					ini.WriteLine("Back=Bg_black.bmp");
					ini.WriteLine("Music=L_bonus.psm");
					ini.WriteLine("Tfmx=T31.tfx");
					ini.WriteLine("Sub=2");
				}
				ini.Close();
				Log.WriteLine("Created level.ini");
			}
			catch (IOException) { }
		}

		public void Undo() {
			try {
				File.Delete("Bonus\\temp\\level.ini");
				Log.WriteLine("Deleted level.ini");
			}
			catch (IOException) { }
		}
	}
}