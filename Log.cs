using System.IO;

class Log {
	private static StreamWriter log = File.CreateText("TStarter.log");

	public static void WriteLine(string line) {
		log.WriteLine(line);
		log.Flush();
	}

	public static void Close() {
		log.Close();
	}
}