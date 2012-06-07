using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;

namespace T_Starter {
	class Entry {
		private string name;
		private long crc;

		public Entry(string name, long crc) {
			this.name = name;
			this.crc = crc;
		}

		public string getName() {
			return name;
		}

		public long getCrc() {
			return crc;
		}
	}

	class T2002Level {
		private string name;
		private List<Entry> entries;

		public T2002Level(string name, List<Entry> entries) {
			this.name = name;
			this.entries = entries;
		}

		public string getName() {
			return name;
		}

		public List<Entry> getEntries() {
			return entries;
		}

		public bool Equals(T2002Level other) {
			if (name != other.name) return false;
			if (entries.Count != other.entries.Count) return false;
			foreach (var entry in entries) {
				var otherentry = other.entries.Find(delegate(Entry e) { return e.getName() == entry.getName(); });
				if (otherentry == null || otherentry.getName() != entry.getName() || otherentry.getCrc() != entry.getCrc()) return false;
			}
			return true;
		}
	}

	public class DownloadManager : System.Windows.Forms.Form {
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private Button button2;
		private Button button3;
		Form1 parent;

		public DownloadManager(Form1 parent) {
			InitializeComponent();
			this.parent = parent;
			this.Closing += new CancelEventHandler(DownloadManager_Closing);
			Reload();
		}

		private void Reload() {
			checkedListBox1.Items.Clear();
			DirectoryInfo dir = new DirectoryInfo("Bonus");
			List<string> levels = new List<string>();

			List<T2002Level> localLevels = new List<T2002Level>();

			foreach (FileInfo file in dir.GetFiles()) {
				if (!file.Name.EndsWith(".zip")) continue;
				ZipFile z = new ZipFile(file.Open(FileMode.Open));
				List<Entry> entries = new List<Entry>();
				for (int i = 0; i < z.Count; ++i) {
					entries.Add(new Entry(z[i].Name, z[i].Crc));
				}
				localLevels.Add(new T2002Level(file.Name, entries));
				z.Close();
			}

			foreach (FileInfo file in dir.GetFiles()) levels.Add(file.Name.Substring(0, file.Name.Length - 4));
			foreach (DirectoryInfo nextdir in dir.GetDirectories()) levels.Add(nextdir.Name);

			WebRequest myRequest = WebRequest.Create("http://turricanforever.de/T2002Levels.xml");//("http://turricanforever.de/levelbase");
			WebResponse myResponse = myRequest.GetResponse();
			System.IO.Stream streamReceive = myResponse.GetResponseStream();
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
			System.IO.StreamReader streamRead = new System.IO.StreamReader(streamReceive, encoding);

			List<T2002Level> remoteLevels = new List<T2002Level>();
			XmlDocument doc = new XmlDocument();
			doc.Load(streamRead);
			var nodes = doc.GetElementsByTagName("Level");
			foreach (XmlNode node in nodes) {
				string levelname = node.Attributes.GetNamedItem("name").Value;
				List<Entry> entries = new List<Entry>();
				foreach (XmlNode entrynode in node.ChildNodes) {
					var entry = new Entry(entrynode.Attributes.GetNamedItem("name").Value, long.Parse(entrynode.Attributes.GetNamedItem("crc").Value));
					entries.Add(entry);
				}
				remoteLevels.Add(new T2002Level(levelname, entries));
			}
			/*
			string page = streamRead.ReadToEnd();
			int start = 0;
			for (string found = FindInbetween(page, "href=\"", "\">", ref start); found != null; found = FindInbetween(page, "href=\"", "\">", ref start))
			{
				if (found.EndsWith(".zip") && found.Contains("t2002"))
				{
					string[] parts = found.Split('/');
					string name = parts[parts.Length - 1]; //found.Substring(7, found.Length - 7 - 4);
					if (name.StartsWith("TStarter") || name.StartsWith("T Starter") || name.StartsWith("T-Starter")) continue;
					if (!levels.Contains(name.Substring(0, name.Length - 4))) checkedListBox1.Items.Add(name, true);
				}
			}*/
			foreach (var level in remoteLevels) {
				bool gotit = false;
				foreach (var locallevel in localLevels) {
					if (locallevel.getName() == level.getName()) {
						if (locallevel.Equals(level)) gotit = true;
						else break;
					}
				}
				if (!gotit) checkedListBox1.Items.Add(level.getName(), true);
			}

			if (checkedListBox1.Items.Count == 0) button1.Enabled = false;
		}

		protected override void Dispose(bool disposing) {
			if (disposing && components != null) components.Dispose();
			base.Dispose( disposing );
		}

		public static string FindInbetween(string s, string s1, string s2, ref int start) {
			int i1 = s.IndexOf(s1, start);
			int i2 = s.IndexOf(s2, i1 + s1.Length);
			start = i2;
			if (i1 == -1 || i2 == -1) return null;
			return s.Substring(i1 + s1.Length, i2 - i1 - s1.Length);
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.Location = new System.Drawing.Point(8, 41);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(360, 214);
			this.checkedListBox1.TabIndex = 0;
			this.checkedListBox1.ThreeDCheckBoxes = true;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(8, 284);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(360, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Download";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 258);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(360, 23);
			this.label1.TabIndex = 2;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(8, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(174, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Check All";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button3.Location = new System.Drawing.Point(194, 12);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(174, 23);
			this.button3.TabIndex = 4;
			this.button3.Text = "Uncheck All";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// DownloadManager
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 312);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.checkedListBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "DownloadManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DownloadManager";
			this.ResumeLayout(false);

		}
		#endregion

		bool downloading = false;
		bool cancel = false;

		private void button1_Click(object sender, System.EventArgs e) {
			if (downloading) {
				lock (this) cancel = true;
			}
			else {
				lock (this) downloading = true;
				button2.Enabled = false;
				button3.Enabled = false;
				checkedListBox1.Enabled = false;
				button1.Text = "Cancel";
				int checkcount = 0;
				for (int i = 0; i < checkedListBox1.Items.Count; ++i) if (checkedListBox1.GetItemChecked(i)) ++checkcount;
				int downcount = 0;
				for (int i = 0; i < checkedListBox1.Items.Count; ++i) {
					if (cancel) break;
					if (checkedListBox1.GetItemChecked(i)) {
						++downcount;
						WebRequest myRequest = WebRequest.Create("http://static.turricanforever.de/levelbase/t2002/" + checkedListBox1.Items[i].ToString());
						WebResponse myResponse = myRequest.GetResponse();
						System.IO.Stream streamReceive = myResponse.GetResponseStream();
						BufferedStream bs = new BufferedStream(streamReceive);

						label1.Text = "Downloaded 0 of " + myResponse.ContentLength.ToString() + " Bytes (File " + downcount + " of " + checkcount + ")";
						label1.Refresh();

						byte[] buffer = new byte[512];
						int read = bs.Read(buffer, 0, buffer.Length);
						int allread = read;
						string filename = "Bonus\\" + checkedListBox1.Items[i].ToString();
						if (File.Exists(filename)) File.Move(filename, filename + ".old");
						FileStream fs = new FileStream("Bonus\\" + checkedListBox1.Items[i].ToString(), FileMode.CreateNew);
						BinaryWriter bw = new BinaryWriter(fs);
						while (read > 0) {
							if (cancel) break;
							bw.Write(buffer, 0, read);
							read = bs.Read(buffer, 0, buffer.Length);
							allread += read;
							label1.Text = "Downloaded " + Math.Round(allread / 1024f) + " of " + Math.Round(myResponse.ContentLength / 1024f).ToString() + " KiB (File " + downcount + " of " + checkcount + ")";
							Application.DoEvents();
						}
						bw.Close();
						fs.Close();
						bs.Close();
						streamReceive.Close();
						myResponse.Close();
						if (cancel) {
							File.Delete("Bonus\\" + checkedListBox1.Items[i].ToString());
							if (File.Exists(filename + ".old")) File.Move(filename + ".old", filename);
						}
						else if (File.Exists(filename + ".old")) File.Delete(filename + ".old");
					}
				}
				Reload();
				button1.Text = "Download";
				this.label1.Text = "";
				cancel = false;
				downloading = false;

				button2.Enabled = true;
				button3.Enabled = true;
				checkedListBox1.Enabled = true;
			}
		}

		private void DownloadManager_Closing(object sender, CancelEventArgs e) {
			parent.Reload();
		}

		//Check All
		private void button2_Click(object sender, EventArgs e) {
			for (int i = 0; i < checkedListBox1.Items.Count; ++i) checkedListBox1.SetItemChecked(i, true);
		}

		//Uncheck All
		private void button3_Click(object sender, EventArgs e) {
			for (int i = 0; i < checkedListBox1.Items.Count; ++i) checkedListBox1.SetItemChecked(i, false);
		}
	}
}