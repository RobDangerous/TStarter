using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace T_Starter {
	class DirectoryPack : LevelPack {
		private DirectoryInfo dir;

		public DirectoryPack(DirectoryInfo dir) {
			this.dir = dir;
			Parse(dir.Name);
		}

		public override Process Load() {
			Task.TaskManager.UndoAll();
			new Task.DirectoryCreator("Bonus\\temp");
			Task.TaskManager.DoAll();
			return Start(dir);
		}
	}
}
