using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T_Starter.Archiver {
	public interface IArchiver {
		void Extract(FileInfo file);
	}
}