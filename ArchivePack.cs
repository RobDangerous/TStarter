using System.IO;

namespace T_Starter {
	class ArchivePack : LevelPack {
		private FileInfo file;

		public ArchivePack(FileInfo file) {
			this.file = file;
			Parse(file.Name.Substring(0, file.Name.LastIndexOf('.')));
		}

		public override void Load() {
			Task.TaskManager.UndoAll();
			new Task.DirectoryCreator("Bonus\\temp");
			new Task.ArchiveExtractor(file);
			Task.TaskManager.DoAll();
			Start(new DirectoryInfo("Bonus\\temp"));
		}
	}
}