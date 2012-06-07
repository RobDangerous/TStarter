using System;
using System.Collections.Generic;
using System.Text;

namespace T_Starter.Task {
	public interface ITask {
		void Do();
		void Undo();
	}
}