namespace GTAV_Downgrader
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxGameDir = new System.Windows.Forms.TextBox();
            this.labelGameDir = new System.Windows.Forms.Label();
            this.folderBrowserGameDir = new System.Windows.Forms.FolderBrowserDialog();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelGameVersionRed = new System.Windows.Forms.Label();
            this.buttonPatch = new System.Windows.Forms.Button();
            this.labelRgscDir = new System.Windows.Forms.Label();
            this.textBoxRgscDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.labelGameVersionGreen = new System.Windows.Forms.Label();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.folderBrowserBackupDir = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonLocalPatch = new System.Windows.Forms.Button();
            this.folderBrowserLocalPatchDir = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonBrowse.Location = new System.Drawing.Point(436, 8);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(26, 22);
            this.buttonBrowse.TabIndex = 0;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxGameDir
            // 
            this.textBoxGameDir.Location = new System.Drawing.Point(100, 9);
            this.textBoxGameDir.Name = "textBoxGameDir";
            this.textBoxGameDir.ReadOnly = true;
            this.textBoxGameDir.Size = new System.Drawing.Size(330, 20);
            this.textBoxGameDir.TabIndex = 1;
            // 
            // labelGameDir
            // 
            this.labelGameDir.AutoSize = true;
            this.labelGameDir.Location = new System.Drawing.Point(13, 12);
            this.labelGameDir.Name = "labelGameDir";
            this.labelGameDir.Size = new System.Drawing.Size(81, 13);
            this.labelGameDir.TabIndex = 2;
            this.labelGameDir.Text = "Game directory:";
            // 
            // folderBrowserGameDir
            // 
            this.folderBrowserGameDir.Description = "Select your Grand Theft Auto V directory (containing GTA5.exe).";
            this.folderBrowserGameDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserGameDir.ShowNewFolderButton = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(12, 73);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(449, 113);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 134;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "MD5 Checksum";
            this.columnHeader2.Width = 254;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Version";
            this.columnHeader3.Width = 57;
            // 
            // labelGameVersionRed
            // 
            this.labelGameVersionRed.AutoSize = true;
            this.labelGameVersionRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameVersionRed.ForeColor = System.Drawing.Color.Red;
            this.labelGameVersionRed.Location = new System.Drawing.Point(48, 223);
            this.labelGameVersionRed.Name = "labelGameVersionRed";
            this.labelGameVersionRed.Size = new System.Drawing.Size(377, 20);
            this.labelGameVersionRed.TabIndex = 4;
            this.labelGameVersionRed.Text = "It seems GTA V is not running on version 1.27";
            this.labelGameVersionRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGameVersionRed.Visible = false;
            // 
            // buttonPatch
            // 
            this.buttonPatch.Enabled = false;
            this.buttonPatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonPatch.Location = new System.Drawing.Point(16, 293);
            this.buttonPatch.Name = "buttonPatch";
            this.buttonPatch.Size = new System.Drawing.Size(443, 40);
            this.buttonPatch.TabIndex = 7;
            this.buttonPatch.Text = "Download and install patch 1.27";
            this.buttonPatch.UseVisualStyleBackColor = true;
            this.buttonPatch.Click += new System.EventHandler(this.buttonPatch_Click);
            // 
            // labelRgscDir
            // 
            this.labelRgscDir.AutoSize = true;
            this.labelRgscDir.Location = new System.Drawing.Point(11, 40);
            this.labelRgscDir.Name = "labelRgscDir";
            this.labelRgscDir.Size = new System.Drawing.Size(83, 13);
            this.labelRgscDir.TabIndex = 4;
            this.labelRgscDir.Text = "RGSC directory:";
            // 
            // textBoxRgscDir
            // 
            this.textBoxRgscDir.Location = new System.Drawing.Point(100, 37);
            this.textBoxRgscDir.Name = "textBoxRgscDir";
            this.textBoxRgscDir.ReadOnly = true;
            this.textBoxRgscDir.Size = new System.Drawing.Size(362, 20);
            this.textBoxRgscDir.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 381);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Made by Dezache - powered by ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(405, 381);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(54, 12);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "SharpZipLib";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.linkLabel2.LinkColor = System.Drawing.Color.Black;
            this.linkLabel2.Location = new System.Drawing.Point(311, 381);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(42, 12);
            this.linkLabel2.TabIndex = 11;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Dezache";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // labelGameVersionGreen
            // 
            this.labelGameVersionGreen.AutoSize = true;
            this.labelGameVersionGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameVersionGreen.ForeColor = System.Drawing.Color.Green;
            this.labelGameVersionGreen.Location = new System.Drawing.Point(63, 223);
            this.labelGameVersionGreen.Name = "labelGameVersionGreen";
            this.labelGameVersionGreen.Size = new System.Drawing.Size(346, 20);
            this.labelGameVersionGreen.TabIndex = 6;
            this.labelGameVersionGreen.Text = "It seems GTA V is running on version 1.27";
            this.labelGameVersionGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGameVersionGreen.Visible = false;
            // 
            // buttonRestore
            // 
            this.buttonRestore.Enabled = false;
            this.buttonRestore.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRestore.Location = new System.Drawing.Point(16, 339);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(219, 23);
            this.buttonRestore.TabIndex = 8;
            this.buttonRestore.Text = "Restore backup";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // folderBrowserBackupDir
            // 
            this.folderBrowserBackupDir.Description = "Select your backed up directory.";
            this.folderBrowserBackupDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserBackupDir.ShowNewFolderButton = false;
            // 
            // buttonLocalPatch
            // 
            this.buttonLocalPatch.Enabled = false;
            this.buttonLocalPatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonLocalPatch.Location = new System.Drawing.Point(241, 339);
            this.buttonLocalPatch.Name = "buttonLocalPatch";
            this.buttonLocalPatch.Size = new System.Drawing.Size(218, 23);
            this.buttonLocalPatch.TabIndex = 9;
            this.buttonLocalPatch.Text = "Install patch from local files";
            this.buttonLocalPatch.UseVisualStyleBackColor = true;
            this.buttonLocalPatch.Click += new System.EventHandler(this.buttonLocalPatch_Click);
            // 
            // folderBrowserLocalPatchDir
            // 
            this.folderBrowserLocalPatchDir.Description = "Select your patch folder (containing GTA5.exe, GTAVLauncher.exe, update.rpf, stea" +
    "m_api64.dll, Social-Club-v1.1.7.8-Setup.exe)";
            this.folderBrowserLocalPatchDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserLocalPatchDir.ShowNewFolderButton = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 402);
            this.Controls.Add(this.buttonLocalPatch);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.labelGameVersionGreen);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxRgscDir);
            this.Controls.Add(this.labelRgscDir);
            this.Controls.Add(this.buttonPatch);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.labelGameDir);
            this.Controls.Add(this.textBoxGameDir);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.labelGameVersionRed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GTA V to 1.27";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxGameDir;
        private System.Windows.Forms.Label labelGameDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserGameDir;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label labelGameVersionRed;
        private System.Windows.Forms.Button buttonPatch;
        private System.Windows.Forms.Label labelRgscDir;
        private System.Windows.Forms.TextBox textBoxRgscDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label labelGameVersionGreen;
        private System.Windows.Forms.Button buttonRestore;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserBackupDir;
        private System.Windows.Forms.Button buttonLocalPatch;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserLocalPatchDir;
    }
}

