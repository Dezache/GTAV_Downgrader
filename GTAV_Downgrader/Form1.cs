using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;

namespace GTAV_Downgrader
{
    public partial class Form1 : Form
    {
        //Storing the MD5 hash for each 1.27 file (and subprocess.exe 1.1.6.8 as well as 1.1.7.8)
        public static string GTA5exe127hash = "D1DFF07E61C11DA6D574B928E3BDE5A4";
        public static string GTAVLauncherexe127hash = "7E7FC8E19887C6F8747A78921E532929";
        public static string steampi64dll127hash = "91212FC3C473AA4730FC02C22DB2BFCB";
        public static string updaterpf127hash = "1F2C8C0FD9F83D6AD82D9A0AA2567562";

        //Other strings
        public string selectedGameDir;
        public string rgscDir;
        public string programVersion = "1.0";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Checking for updates...";
            this.Refresh();
            //Checking for updates
            string latestVersion = ProgramLatestVersion();
            if (programVersion != latestVersion && latestVersion != null)
            {
                DialogResult dialogResult = MessageBox.Show($"There is an update available (version {latestVersion}). It is highly recommended to update to the latest version.\nWould you like to be redirected to the download page?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start("https://dezache.github.io/programs/gtav_downgrader/");
                }
            }

            //Setting rgscDir value
            toolStripStatusLabel1.Text = "Looking for RGSC directory...";
            rgscDir = RGSCDirectory();
            if (rgscDir == null)
            {
                MessageBox.Show("Downgrader couldn't find Rockstar Games Social Club. Exiting program.", "RGSC not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                textBoxRgscDir.Text = rgscDir;
            }

            //Deleting eventual leftovers from last run - "extraction" folder and downloaded patch
            if (Directory.Exists("extraction")) { Directory.Delete("extraction", true); }
            if (File.Exists("patch.zip")) { File.Delete("patch.zip"); }

            //Automatic GTA V directory detection
            selectedGameDir = GTADir();
            if (selectedGameDir == null)
            {
                toolStripStatusLabel1.Text = "Ready."; //if the program couldn't find GTA's directory, it just ignores it
            }
            else
            {
                textBoxGameDir.Text = selectedGameDir;
                RepopulateList();
            }
            
                
                
            
        }

        public string FileMD5(string filename) //Gets the MD5 hash for a given file, and return it as a string
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty);
                }
            }
        }

        public string RGSCVersion() //Gets RGSC version from Registry
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Rockstar Games\\Rockstar Games Social Club"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("Version");
                        if (o != null)
                        {
                            string version = o.ToString();
                            return version;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "Error";
        }

        public string RGSCDirectory() //Gets RGSC directory from Registry
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Rockstar Games\\Rockstar Games Social Club"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("InstallFolder");
                        if (o != null)
                        {
                            string folder = o.ToString();
                            return folder;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public string GTADir() //Gets GTA directory from Registry
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Rockstar Games\\GTAV"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("InstallFolderSteam");
                        if (o != null)
                        {
                            string path = o.ToString();
                            string endOfPath = path.Substring(path.Length - 5, 5);
                            if (endOfPath == "\\GTAV") //It seems Rockstar added an unnecessary "\GTAV" at the end of the directory, so we remove it
                            {
                                return path.Substring(0, path.Length - 5);
                            }
                            else
                                return path;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public string ProgramLatestVersion() //Gets program latest version number from the internet
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://dezache.github.io/programs/gtav_downgrader/version.txt");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                client.Dispose();
                stream.Close();
                reader.Close();
                return content;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not check for updates.\n" + ex.Message);
                return null;
            }
        }
        
        static bool ProcessIsRunning(string nameSubstring) //Checks if a process is running (useful when waiting for RGSC uninstallation to finish before launching reinstallation)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName == nameSubstring)
                {
                    return true;
                }
            }
            return false;
        }

        public void RepopulateList()
        {
            //Reset the "verdict" label, disable the patch button, clear/reset the listView
            labelGameVersionRed.Visible = false;
            labelGameVersionGreen.Visible = false;
            buttonPatch.Enabled = false;
            buttonLocalPatch.Enabled = false;
            buttonRestore.Enabled = false;

            listView1.Items.Clear();
            listView1.Items.Add("GTA5.exe");
            listView1.Items.Add("GTAVLauncher.exe");
            listView1.Items.Add("update.rpf");
            listView1.Items.Add("steam_api64.dll");
            listView1.Items.Add("RGSC");

            toolStripStatusLabel1.Text = "Analyzing GTA V files...";
            this.Refresh();
            Cursor.Current = Cursors.WaitCursor;
            //Serious shit starts happening here
            //Filling the game directory textBox, and making strings for each file's path
            
            string GTA5exePath = $@"{selectedGameDir}\GTA5.exe";
            string GTAVLauncherexePath = $@"{selectedGameDir}\GTAVLauncher.exe";
            string steampi64dllPath = $@"{selectedGameDir}\steam_api64.dll";
            string updaterpfPath = $@"{selectedGameDir}\update\update.rpf";

            //Making a string for each calculated MD5 hash
            string currGTA5hash = FileMD5(GTA5exePath);
            string currGTAVLauncherhash = FileMD5(GTAVLauncherexePath);
            string currupdaterpfhash = FileMD5(updaterpfPath);
            string currsteamapi64dllhash = FileMD5(steampi64dllPath);


            //Comparing current version MD5 to 1.27 MD5 (filling the "MD5 Checksum" and "Version" columns)
            listView1.Items[0].SubItems.Add(currGTA5hash);
            listView1.Items[0].UseItemStyleForSubItems = false;
            if (currGTA5hash == GTA5exe127hash)
            {
                listView1.Items[0].SubItems.Add("1.27", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else
            {
                listView1.Items[0].SubItems.Add("Not 1.27", Color.Black, Color.IndianRed, DefaultFont);
            }

            listView1.Items[1].SubItems.Add(currGTAVLauncherhash);
            listView1.Items[1].UseItemStyleForSubItems = false;
            if (currGTAVLauncherhash == GTAVLauncherexe127hash)
            {
                listView1.Items[1].SubItems.Add("1.27", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else
            {
                listView1.Items[1].SubItems.Add("Not 1.27", Color.Black, Color.IndianRed, DefaultFont);
            }

            listView1.Items[2].SubItems.Add(currupdaterpfhash);
            listView1.Items[2].UseItemStyleForSubItems = false;
            if (currupdaterpfhash == updaterpf127hash)
            {
                listView1.Items[2].SubItems.Add("1.27", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else
            {
                listView1.Items[2].SubItems.Add("Not 1.27", Color.Black, Color.IndianRed, DefaultFont);
            }

            listView1.Items[3].SubItems.Add(currsteamapi64dllhash);
            listView1.Items[3].UseItemStyleForSubItems = false;
            if (currsteamapi64dllhash == steampi64dll127hash)
            {
                listView1.Items[3].SubItems.Add("1.27", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else
            {
                listView1.Items[3].SubItems.Add("Not 1.27", Color.Black, Color.IndianRed, DefaultFont);
            }

            listView1.Items[4].SubItems.Add("");
            listView1.Items[4].UseItemStyleForSubItems = false;
            string rgscVersion = RGSCVersion();
            if (rgscVersion == "1.1.6.8")
            {
                listView1.Items[4].SubItems.Add("1.1.6.8", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else if (rgscVersion == "1.1.7.8")
            {
                listView1.Items[4].SubItems.Add("1.1.7.8", Color.Black, Color.FromArgb(128, 188, 58), DefaultFont);
            }
            else
            {
                listView1.Items[4].SubItems.Add(rgscVersion, Color.Black, Color.IndianRed, DefaultFont);
            }


            //Displaying the verdict depending on the results for each file
            if (listView1.Items[0].SubItems[2].Text == "Not 1.27" || listView1.Items[1].SubItems[2].Text == "Not 1.27" || listView1.Items[2].SubItems[2].Text == "Not 1.27" || listView1.Items[3].SubItems[2].Text == "Not 1.27" || (listView1.Items[4].SubItems[2].Text != "1.1.6.8" && listView1.Items[4].SubItems[2].Text != "1.1.7.8"))
            {
                labelGameVersionRed.Visible = true;
                buttonPatch.Enabled = true;
                buttonLocalPatch.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                labelGameVersionGreen.Visible = true;
                buttonRestore.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            toolStripStatusLabel1.Text = "Ready.";
        }
        
        private void buttonBrowse_Click(object sender, EventArgs e) //Triggered when the "..." button is clicked
        {
            if (selectedGameDir != "") { folderBrowserGameDir.SelectedPath = selectedGameDir; }
            //Launch the folder browser
            DialogResult result = folderBrowserGameDir.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Checking for every file to patch in the selected folder
                if (!File.Exists($@"{folderBrowserGameDir.SelectedPath}\GTA5.exe"))
                {
                    MessageBox.Show("GTA5.exe not found. Please try again, or verify integrity of game files.", "File not found", MessageBoxButtons.OK ,MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{folderBrowserGameDir.SelectedPath}\GTAVLauncher.exe"))
                {
                    MessageBox.Show("GTAVLauncher.exe not found. Please try again, or verify integrity of game files.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{folderBrowserGameDir.SelectedPath}\steam_api64.dll"))
                {
                    MessageBox.Show("steam_api64.dll not found. Please try again, or verify integrity of game files.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{folderBrowserGameDir.SelectedPath}\update\update.rpf"))
                {
                    MessageBox.Show("update\\update.rpf not found. Please try again, or verify integrity of game files.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    textBoxGameDir.Text = folderBrowserGameDir.SelectedPath;
                    selectedGameDir = folderBrowserGameDir.SelectedPath;
                    RepopulateList();
                }
            }
        }

        private void buttonPatch_Click(object sender, EventArgs e)
        {
            buttonPatch.Enabled = false;
            buttonLocalPatch.Enabled = false;
            buttonRestore.Enabled = false;
            DialogResult dialogResult = MessageBox.Show("The downgrader will now download and install patch 1.27 (~450MB). The download might take some time, depending on your download speeds. If needed, you will be prompted to uninstall and reinstall RGSC. Please do not close the downgrader until the completion popup appears. Your current game files will be backed up in \"Backup_before_downgrade\" in your game directory.", "Patch", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Cancel)
            {
                buttonPatch.Enabled = true;
                return;
            }
            else if (dialogResult == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "Backing up game files...";
                this.Refresh();
                Cursor.Current = Cursors.WaitCursor;
                //Backup
                if (!Directory.Exists($@"{selectedGameDir}\Backup_before_downgrade"))
                {
                    Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade");
                    Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade\update");
                    File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_downgrade\GTA5.exe");
                    File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_downgrade\GTAVLauncher.exe");
                    File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_downgrade\steam_api64.dll");
                    File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_downgrade\update\update.rpf");
                }
                //If there already exists a Backup_before_downgrade folder, create a new one with a timestamp added at the end of the name and backup there to avoid conflicts
                else
                {
                    string unixTimestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

                    Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}");
                    Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\update");
                    File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\GTA5.exe");
                    File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\GTAVLauncher.exe");
                    File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\steam_api64.dll");
                    File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\update\update.rpf");
                }

                toolStripStatusLabel1.Text = "Downloading patch...";
                this.Refresh();
                //Download the patch from my personal dropbox folder (it's the same as darkviper's, I just made mine in case he modifies/deletes his, so the program always works)
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri("https://www.dropbox.com/sh/gzetthtvk7k38df/AAD6PLhMCTSWpwCKBw8jdL6ma?dl=1"), "patch.zip");

                toolStripStatusLabel1.Text = "Extracting patch...";
                this.Refresh();
                //Extract the patch to a created "extraction" folder
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip("patch.zip", "extraction", null);

                toolStripStatusLabel1.Text = "Installing patch...";
                this.Refresh();
                //Moving each downloaded game file to actual game directory
                File.Copy(@"extraction\GTA5.exe", $@"{selectedGameDir}\GTA5.exe", true);
                File.Copy(@"extraction\GTAVLauncher.exe", $@"{selectedGameDir}\GTAVLauncher.exe", true);
                File.Copy(@"extraction\steam_api64.dll", $@"{selectedGameDir}\steam_api64.dll", true);
                File.Copy(@"extraction\update.rpf", $@"{selectedGameDir}\update\update.rpf", true);

                //Checking if it's needed to update RGSC
                if ((listView1.Items[4].SubItems[2].Text != "1.1.6.8" && listView1.Items[4].SubItems[2].Text != "1.1.7.8"))
                {
                    toolStripStatusLabel1.Text = "Patching RGSC...";
                    this.Refresh();
                    var process = new Process { StartInfo = new ProcessStartInfo($@"{rgscDir}\uninstallRGSCRedistributable.exe") };
                    process.Start();
                    process.WaitForExit();
                    while (ProcessIsRunning("Au_"))
                    {
                        //Looping nothing while RGSC uninstallation process is running (uninstaller's process name is "Au_")
                    }
                    var process2 = new Process { StartInfo = new ProcessStartInfo(@"extraction\Social-Club-v1.1.7.8-Setup.exe") };
                    process2.Start();
                    process2.WaitForExit();
                    while (ProcessIsRunning("Social-Club-v1.1.7.8-Setup.exe"))
                    {
                        //Looping nothing while RGSC setup process is running
                    }
                }

                //Resetting again
                textBoxGameDir.Text = String.Empty;
                labelGameVersionRed.Visible = false;
                labelGameVersionGreen.Visible = false;
                buttonPatch.Enabled = false;
                buttonRestore.Enabled = false;
                buttonLocalPatch.Enabled = false;
                listView1.Items.Clear();

                toolStripStatusLabel1.Text = "Cleaning up...";
                this.Refresh();
                //Deleting "extraction" folder and downloaded patch
                if (File.Exists("patch.zip")) { File.Delete("patch.zip"); }
                Thread.Sleep(2000); //2 second sleep to avoid deleting a still active Social-Club-v1.1.7.8-Setup.exe process, which would cause an error
                if (Directory.Exists("extraction")) { Directory.Delete("extraction", true); }


                //Finish!
                toolStripStatusLabel1.Text = "Ready.";
                this.Refresh();
                Cursor.Current = Cursors.Default;

                MessageBox.Show("Downgrade complete!\nTo roll back to the backed up version, use the Restore button.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            

        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            folderBrowserBackupDir.SelectedPath = selectedGameDir; //Start the dialog from the previously selected GTA V directory 
            DialogResult result = folderBrowserBackupDir.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string backupDir = folderBrowserBackupDir.SelectedPath;

                //Checking for every file to backup in the selected folder
                if (!File.Exists($@"{backupDir}\GTA5.exe"))
                {
                    MessageBox.Show("GTA5.exe not found in selected directory. Restore aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{backupDir}\GTAVLauncher.exe"))
                {
                    MessageBox.Show("GTAVLauncher.exe not found in selected directory. Restore aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{backupDir}\steam_api64.dll"))
                {
                    MessageBox.Show("steam_api64.dll not found in selected directory. Restore aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{backupDir}\update\update.rpf"))
                {
                    MessageBox.Show("update\\update.rpf not found in selected directory. Restore aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else //none of the files are missing
                {
                    DialogResult dialogResult = MessageBox.Show("The downgrader will now restore your backed up game version. You will be prompted to uninstall and install RGSC. Please do not close the downgrader until the completion popup appears.\nDo you want to backup your current - patched - game files?", "Restore", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (dialogResult == DialogResult.Yes)
                    {
                        toolStripStatusLabel1.Text = "Backing up game files...";
                        this.Refresh();
                        //Backup
                        if (!Directory.Exists($@"{selectedGameDir}\Backup_before_restore"))
                        {
                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_restore");
                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_restore\update");
                            File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_restore\GTA5.exe");
                            File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_restore\GTAVLauncher.exe");
                            File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_restore\steam_api64.dll");
                            File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_restore\update\update.rpf");
                        }

                        //If there already exists a Backup_before_restore folder, create a new one with a timestamp added at the end of the name and backup there to avoid conflicts
                        else
                        {
                            string unixTimestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}");
                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}\update");
                            File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}\GTA5.exe");
                            File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}\GTAVLauncher.exe");
                            File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}\steam_api64.dll");
                            File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_restore_{unixTimestamp}\update\update.rpf");
                        }
                    }

                    toolStripStatusLabel1.Text = "Restoring game files...";
                    this.Refresh();
                    //Restoring files
                    File.Copy($@"{backupDir}\GTA5.exe", $@"{selectedGameDir}\GTA5.exe", true);
                    File.Copy($@"{backupDir}\GTAVLauncher.exe", $@"{selectedGameDir}\GTAVLauncher.exe", true);
                    File.Copy($@"{backupDir}\steam_api64.dll", $@"{selectedGameDir}\steam_api64.dll", true);
                    File.Copy($@"{backupDir}\update\update.rpf", $@"{selectedGameDir}\update\update.rpf", true);

                    //Delete empty backup folder
                    Directory.Delete(backupDir, true);

                    toolStripStatusLabel1.Text = "Restoring RGSC...";
                    this.Refresh();
                    //Restore recent RGSC
                    if (File.Exists($@"{selectedGameDir}\Installers\Social-Club-Setup.exe"))
                    {
                        var process3 = new Process { StartInfo = new ProcessStartInfo($@"{rgscDir}\uninstallRGSCRedistributable.exe") };
                        process3.Start();
                        process3.WaitForExit();
                        while (ProcessIsRunning("Au_"))
                        {
                            //Looping nothing while RGSC uninstallation process is running (uninstaller's process name is "Au_")
                        }

                        var process4 = new Process { StartInfo = new ProcessStartInfo($@"{selectedGameDir}\Installers\Social-Club-Setup.exe") };
                        process4.Start();
                        process4.WaitForExit();
                        while (ProcessIsRunning("Social-Club-Setup.exe"))
                        {
                            //Looping nothing while RGSC setup process is running
                        }
                    }
                    else
                    {
                        MessageBox.Show("Couldn't find an RGSC version to restore. You will have to restore it manually by following these steps:\n1. Verify integrity of game files on Steam\n2. Uninstall Rockstar Games Social Club from the Control panel\n3. Go to your game directory, open the Installers folder and launch Social-Club-Setup\n4. Follow the instructions to install the latest RGSC version, and the game will be ready to be played", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //Resetting again
                    textBoxGameDir.Text = String.Empty;
                    labelGameVersionRed.Visible = false;
                    labelGameVersionGreen.Visible = false;
                    buttonPatch.Enabled = false;
                    buttonRestore.Enabled = false;
                    buttonLocalPatch.Enabled = false;
                    listView1.Items.Clear();


                    //Done
                    toolStripStatusLabel1.Text = "Ready.";
                    this.Refresh();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Restore complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }

            }
        }

        private void buttonLocalPatch_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserLocalPatchDir.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string patchDir = folderBrowserLocalPatchDir.SelectedPath;

                //Checking for every file to backup in the selected folder
                if (!File.Exists($@"{patchDir}\GTA5.exe"))
                {
                    MessageBox.Show("GTA5.exe not found in selected directory. Patching aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{patchDir}\GTAVLauncher.exe"))
                {
                    MessageBox.Show("GTAVLauncher.exe not found in selected directory. Patching aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{patchDir}\steam_api64.dll"))
                {
                    MessageBox.Show("steam_api64.dll not found in selected directory. Patching aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (!File.Exists($@"{patchDir}\update\update.rpf") && !File.Exists($@"{patchDir}\update.rpf"))
                {
                    MessageBox.Show("update.rpf not found in selected directory. Patching aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
                else if (!File.Exists($@"{patchDir}\Social-Club-v1.1.7.8-Setup.exe") && (listView1.Items[4].SubItems[2].Text != "1.1.6.8" && listView1.Items[4].SubItems[2].Text != "1.1.7.8"))
                {
                    MessageBox.Show("Social-Club-v1.1.7.8-Setup.exe not found in selected directory. Patching aborted.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else //none of the files are missing
                {
                    DialogResult dialogResult = MessageBox.Show("The downgrader will now install patch 1.27. If needed, you will be prompted to uninstall and install RGSC. Please do not close the downgrader until the completion popup appears. Your current game files will be backed up in \"Backup_before_downgrade\" in your game directory.", "Patch", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    toolStripStatusLabel1.Text = "Backing up game files...";
                    this.Refresh();
                    //Backup
                    if (!Directory.Exists($@"{selectedGameDir}\Backup_before_downgrade"))
                    {
                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade");
                            Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade\update");
                            File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_downgrade\GTA5.exe");
                            File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_downgrade\GTAVLauncher.exe");
                            File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_downgrade\steam_api64.dll");
                            File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_downgrade\update\update.rpf");
                            
                    }

                    //If there already exists a Backup_before_downgrade folder, create a new one with a timestamp added at the end of the name and backup there to avoid conflicts
                    else
                    {
                        string unixTimestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}");
                        Directory.CreateDirectory($@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\update");
                        File.Copy($@"{selectedGameDir}\GTA5.exe", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\GTA5.exe");
                        File.Copy($@"{selectedGameDir}\GTAVLauncher.exe", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\GTAVLauncher.exe");
                        File.Copy($@"{selectedGameDir}\steam_api64.dll", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\steam_api64.dll");
                        File.Copy($@"{selectedGameDir}\update\update.rpf", $@"{selectedGameDir}\Backup_before_downgrade_{unixTimestamp}\update\update.rpf");
                    }


                    toolStripStatusLabel1.Text = "Installing patch...";
                    this.Refresh();
                    //Install patch
                    File.Copy($@"{patchDir}\GTA5.exe", $@"{selectedGameDir}\GTA5.exe", true);
                    File.Copy($@"{patchDir}\GTAVLauncher.exe", $@"{selectedGameDir}\GTAVLauncher.exe", true);
                    File.Copy($@"{patchDir}\steam_api64.dll", $@"{selectedGameDir}\steam_api64.dll", true);
                    if (File.Exists($@"{patchDir}\update\update.rpf")) { File.Copy($@"{patchDir}\update\update.rpf", $@"{selectedGameDir}\update\update.rpf", true); }
                    else if (File.Exists($@"{patchDir}\update.rpf")) { File.Copy($@"{patchDir}\update.rpf", $@"{selectedGameDir}\update\update.rpf", true); }

                    toolStripStatusLabel1.Text = "Patching RGSC...";
                    this.Refresh();
                    //RGSC
                    if ((listView1.Items[4].SubItems[2].Text != "1.1.6.8" && listView1.Items[4].SubItems[2].Text != "1.1.7.8"))
                    {
                        var process5 = new Process { StartInfo = new ProcessStartInfo($@"{rgscDir}\uninstallRGSCRedistributable.exe") };
                        process5.Start();
                        process5.WaitForExit();
                        while (ProcessIsRunning("Au_"))
                        {
                            //Looping nothing while RGSC uninstallation process is running (uninstaller's process name is "Au_")
                        }

                        var process6 = new Process { StartInfo = new ProcessStartInfo($@"{patchDir}\Social-Club-v1.1.7.8-Setup.exe") };
                        process6.Start();
                        process6.WaitForExit();
                        while (ProcessIsRunning("Social-Club-v1.1.7.8-Setup.exe"))
                        {
                            //Looping nothing while RGSC setup process is running
                        }
                    }



                    //Resetting again
                    textBoxGameDir.Text = String.Empty;
                    labelGameVersionRed.Visible = false;
                    labelGameVersionGreen.Visible = false;
                    buttonPatch.Enabled = false;
                    buttonRestore.Enabled = false;
                    buttonLocalPatch.Enabled = false;
                    listView1.Items.Clear();

                    //Done
                    toolStripStatusLabel1.Text = "Ready.";
                    this.Refresh();
                    MessageBox.Show("Downgrade complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cursor.Current = Cursors.Default;
                }

            }
        }

        #region Clickable labels
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://icsharpcode.github.io/SharpZipLib/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.speedrun.com/user/Dezache");
        }
        #endregion

        
    }
}
