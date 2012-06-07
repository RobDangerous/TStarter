using System;
using System.Collections.Generic;
using System.Text;

namespace T_Starter.Archiver {
	public class ArchiveException : Exception {
		private Exception ex;

		public ArchiveException(Exception ex) {
			this.ex = ex;
		}

		public override string Message {
			get {
				return ex.Message;
			}
		}
	}
}