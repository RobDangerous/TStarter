using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace T_Starter.Task {
	public class TaskManager {
		const string securityfile = "ts29rescue.dat";
		static IList<ITask> todo = new List<ITask>();
		static Stack<ITask> done = new Stack<ITask>();

		static TaskManager() {
			if (File.Exists(securityfile)) {
				using (FileStream fs = new FileStream(securityfile, FileMode.Open)) {
					BinaryFormatter binFormatter = new BinaryFormatter();
					done = (Stack<ITask>)binFormatter.Deserialize(fs);
				}
			}
		}

		public static void Add(ITask task) {
			todo.Add(task);
		}

		public static void DoAll() {
			foreach (ITask task in todo) {
				task.Do();
				done.Push(task);
			}
			using (FileStream fs = new FileStream(securityfile, FileMode.Create)) {
				BinaryFormatter serializer = new BinaryFormatter();
				serializer.Serialize(fs, done);
			}
			todo.Clear();
		}

		public static void UndoAll() {
			foreach (ITask task in done) task.Undo();
			done.Clear();
			if (File.Exists(securityfile)) File.Delete(securityfile);
		}
	}
}