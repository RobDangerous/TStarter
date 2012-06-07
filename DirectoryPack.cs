using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter {
	class DirectoryPack : LevelPack {
		private DirectoryInfo dir;

		public DirectoryPack(DirectoryInfo dir) {
			this.dir = dir;
			Parse(dir.Name);
		}

		public override void Load() {
			Task.TaskManager.UndoAll();
			new Task.DirectoryCreator("Bonus\\temp");
			Task.TaskManager.DoAll();
			Start(dir);
		}
	}
}