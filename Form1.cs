using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace T_Starter {
	public class Form1 : System.Windows.Forms.Form {
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label author;
		private System.Windows.Forms.Label date;
		private System.Windows.Forms.Button button2;
		private const string original = "T2002 Original";
		private ProgressBar progressBar1;
		static Mutex mut;
		
		public static Form1 Instance { get; private set; }

		private Form1() {
			InitializeComponent();
			Reload();
			this.Closing += new CancelEventHandler(Form1_Closing);
		}

		public void Add(object item) {
			comboBox1.Items.Add(item);
		}

		protected override void Dispose(bool disposing) {
			if (disposing && components != null) components.Dispose();
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.author = new System.Windows.Forms.Label();
			this.date = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Location = new System.Drawing.Point(11, 8);
			this.comboBox1.MaxDropDownItems = 16;
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(168, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(11, 75);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Start T2002";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// author
			// 
			this.author.Location = new System.Drawing.Point(19, 40);
			this.author.Name = "author";
			this.author.Size = new System.Drawing.Size(160, 16);
			this.author.TabIndex = 2;
			this.author.Text = "Author:";
			// 
			// date
			// 
			this.date.Location = new System.Drawing.Point(19, 56);
			this.date.Name = "date";
			this.date.Size = new System.Drawing.Size(160, 16);
			this.date.TabIndex = 3;
			this.date.Text = "Date:";
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(104, 75);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "Download";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(11, 10);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(168, 17);
			this.progressBar1.TabIndex = 5;
			this.progressBar1.Visible = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 103);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.date);
			this.Controls.Add(this.author);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "T Starter 2.7";
			this.ResumeLayout(false);

		}
		#endregion

		/*
		   try { // Mono mag's nicht
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
				this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			}
			catch (Exception) { }
		 */

		[STAThread]
		static void Main() {
			bool mutexWasCreated;
			mut = new Mutex(true, "T Starter 2", out mutexWasCreated);
			if (!mutexWasCreated) return;

			try {
				XmlDocument doc = new XmlDocument();
				doc.Load("T2002Levels.xml");
				OnlineLevels.levels.Clear();
				var nodes = doc.GetElementsByTagName("Level");
				foreach (XmlNode node in nodes) {
					string levelname = node.Attributes.GetNamedItem("name").Value;

					OnlineLevel level = new OnlineLevel();
					level.name = levelname;
					level.author = node.Attributes.GetNamedItem("author").Value;
					level.date = node.Attributes.GetNamedItem("date").Value;
					OnlineLevels.levels.Add(level);
				}
			}
			catch (Exception) {

			}

			Application.EnableVisualStyles();
			Application.Run(Instance = new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e) {
			try {
				LevelPack pack = (LevelPack) comboBox1.SelectedItem;
				pack.Load();
			}
			catch (InvalidCastException) {
				try {
					Task.TaskManager.UndoAll();
					Process.Start("T2002E.exe");
				}
				catch (System.ComponentModel.Win32Exception) {
					MessageBox.Show(this, "Could not start T2002E.exe - please install T2002 and the T2002-level-editor and put the T Starter in it's directory.", "Error");
				}
			}
			catch (Archiver.ArchiveException ex) {
				MessageBox.Show(this, "Archive exception: " + ex.Message, "Error");
			}
			catch (System.ComponentModel.Win32Exception) {
				MessageBox.Show(this, "Could not start T2002E.exe - please install T2002 and the T2002-level-editor and put the T Starter in it's directory.", "Error");
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (comboBox1.SelectedItem is LevelPack) {
				LevelPack pack = (LevelPack) comboBox1.SelectedItem;
				author.Text = "Author: " + pack.Author;
				date.Text = "Date: " + pack.Date;
			}
			else {
				author.Text = "Author: Pekaro";
				date.Text = "Date: 2002";
			}
		}

		private void button2_Click(object sender, System.EventArgs e) {
			try
			{
				new DownloadManager(this).ShowDialog(this);
			}
			catch (System.Net.WebException)
			{
				MessageBox.Show("Could not connect to turricanforever.de");
			}
		}

		public void Reload() {
			comboBox1.Items.Clear();
			Add(original);
			comboBox1.SelectedItem = original;

			DirectoryInfo bonus = new DirectoryInfo("Bonus");
			if (!bonus.Exists) bonus.Create();

			var found = new List<string>();
			DirectoryInfo[] dirs = bonus.GetDirectories();
			foreach (DirectoryInfo dir in dirs) {
				if (dir.Name != "temp") {
					LevelPack pack = new DirectoryPack(dir);
					Add(pack);
					found.Add(dir.Name);
				}
			}
			FileInfo[] files = bonus.GetFiles();
			foreach (FileInfo file in files) {
				if (!found.Contains(file.Name.Substring(0, file.Name.Length - 4))) {
					LevelPack pack = new ArchivePack(file);
					Add(pack);
				}
			}
		}

		private void Form1_Closing(object sender, CancelEventArgs e) {
			Task.TaskManager.UndoAll();
			Log.Close();
			mut.ReleaseMutex();
		}

		public void SetPercentage(float percentage)
		{
			progressBar1.Value = (int)(percentage * 100);
		}

		public void PrepareExtraction()
		{
			comboBox1.Hide();
			button1.Enabled = false;
			button2.Enabled = false;
			//button1.Hide();
			//button2.Hide();
			//progressBar1.Location = button1.Location;
			progressBar1.Show();
		}

		public void FinishExtraction()
		{
			progressBar1.Hide();
			comboBox1.Show();
			button1.Enabled = true;
			button2.Enabled = true;
			//button1.Show();
			//button2.Show();
		}
	}
}