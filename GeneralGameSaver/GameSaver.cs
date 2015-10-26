using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralGameSaver
{
    public partial class GameSaver : Form
    {
        private AppSettings _appSettings;
        private readonly List<Icon> _iconList;
        public GameSaver()
        {
            InitializeComponent(); 
            _iconList = new List<Icon>();
            loadIcons();
            _appSettings = new AppSettings();
            if (IsSettingsReady())
            {
                StartStop(_appSettings.AutoStart);
            }
            updateList();
            updateUI();
        }

        private enum ImagesEnum { GameSaverStop, GameSaver1, GameSaver2, GameSaver3, GameSaver4, GameSaver5, GameSaver6, GameSaver7, GameSaver8, GameSaver9, END }
        private void loadIcons()
        {
            for (ImagesEnum i = 0; i < ImagesEnum.END; i++)
            {
                var nm = i.ToString();
                var img = (Bitmap)Properties.Resources.ResourceManager.GetObject(nm);
                var ico = Icon.FromHandle(img.GetHicon());
                _iconList.Add(ico);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void setCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialogSelectCatalog.ShowDialog();
            if (res != DialogResult.OK) return;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var parDialog = new Parameters();
            var res = parDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                _appSettings = new AppSettings();
                updateUI();
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            saveGame();
        }

        private void saveGame()
        {
            if (string.IsNullOrEmpty(_appSettings.SaveCatalog) || string.IsNullOrEmpty(_appSettings.GameCatalog))
                return;
            var dt = DateTime.MinValue;
            var path = _appSettings.GameCatalog;
            using (var zipfile = new Ionic.Zip.ZipFile())
            {
                RecursiveArchivate(path, zipfile, "", ref dt);
                if (dt <= _lastArchiveTime) return;
                var name = Path.GetFileNameWithoutExtension(_appSettings.GameCatalog);
                var filename = string.Format("{0}\\savegame_{1}_{2}.zip", _appSettings.SaveCatalog, dt.ToString("yyMMdd-HHmmss"), name);
                zipfile.Save(filename);
                File.SetLastWriteTime(filename, dt);
            }
            updateList();
            updateUI();
        }
        private static void RecursiveArchivate(string path, Ionic.Zip.ZipFile zip, string relPath, ref DateTime dt)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                //if (!CheckFile(file)) continue;
                var fdt = File.GetLastWriteTime(file);
                if (fdt > dt) dt = File.GetLastWriteTime(file);
                zip.AddFile(file, relPath);
            }
            foreach (string directory in Directory.GetDirectories(path))
            {
                var dirName = new DirectoryInfo(directory).Name;
                RecursiveArchivate(directory, zip, string.Format(@"{0}\{1}", relPath, dirName), ref dt);
            }
        }

        private Dictionary<string, string> GetArchives()
        {
            var list = new Dictionary<string, string>();
            var sf = _appSettings.SaveCatalog;
            if (string.IsNullOrEmpty(sf)) return list;
            foreach (var file in Directory.GetFiles(sf))
            {
                if (file.Contains("savegame_"))
                {
                    var name = Path.GetFileName(file);
                    list[name] = file;
                }
            }
            return list;
        }

        private DateTime _lastArchiveTime = DateTime.MinValue;

        private void updateUI()
        {
            var setReady = IsSettingsReady();
            bStart.Enabled = !timer1.Enabled && setReady;
            bStop.Enabled = timer1.Enabled;
            bSave.Enabled = setReady;
            bRestore.Enabled = setReady && listView1.SelectedItems.Count > 0;
            bRemove.Enabled = listView1.SelectedItems.Count > 0;
            toolStripStatusLabel1.Text = timer1.Enabled ? "Working..." : "Stopped";
            toolStripStatusLabel2.Text = _appSettings.GameCatalog;
            if (setReady) notifyIcon1.Icon = _iconList[0];
            cmStart.Enabled = !timer2.Enabled;
            cmStop.Enabled = timer2.Enabled;
            startToolStripMenuItem.Enabled = !timer2.Enabled;
            stopToolStripMenuItem.Enabled = timer2.Enabled;
        }

        private bool IsSettingsReady()
        {
            return (!string.IsNullOrEmpty(_appSettings.GameCatalog) && Directory.Exists(_appSettings.GameCatalog) &&
                    !string.IsNullOrEmpty(_appSettings.SaveCatalog) && Directory.Exists(_appSettings.SaveCatalog));
        }

        private void updateList()
        {
            var ci = Thread.CurrentThread.CurrentUICulture;
            var list = GetArchives();
            var items = new List<ListViewItem>();
            var lastDt = DateTime.MinValue;
            var groups = new Dictionary<string, List<string>>();
            var lvGroups = new Dictionary<string, ListViewGroup>();
            var re = new Regex(@"^savegame_([\d-]+)_(.+).zip");
            foreach (var key in list.Keys.OrderBy(r=>r))
            {
                try
                {
                    var m = re.Match(key);
                    if (!m.Success || m.Groups.Count < 3) continue;
                    var group = m.Groups[2].Value;
                    var date = m.Groups[1].Value;
                    //var parts = key.Split(new []{'_'}, 3);
                    //if (parts.Length < 3) continue;
                    var dt = DateTime.ParseExact(date, "yyMMdd-HHmmss", ci);
                    if (dt > lastDt) lastDt = dt;

//                    var group = parts[2];
                    if (!groups.ContainsKey(group))
                    {
                        groups[group]= new List<string>();
                        lvGroups[group] = new ListViewGroup(group);
                    }
                    groups[group].Add(key);
                    var li = new ListViewItem(key);
                    li.SubItems.Add(dt.ToString("g"));
                    li.Group = lvGroups[group];
                    li.Tag = list[key];
                    items.Add(li);
                }
                catch
                {
                }
            }
            var l = groups.Keys.ToArray();
            foreach (var group in l)
            {
                if (groups[group].Count > _appSettings.NumberOfFiles)
                {
                    foreach (var file in groups[group].OrderByDescending(r=>r).Skip(_appSettings.NumberOfFiles))
                    {
                        File.Delete(list[file]);
                    }
                    groups[group] = groups[group].OrderByDescending(r => r).Take(_appSettings.NumberOfFiles).ToList();
                }
            }
            if (lastDt > _lastArchiveTime)
            {
                listView1.Items.Clear();
                listView1.Groups.AddRange(lvGroups.Values.ToArray());
                listView1.Items.AddRange(items.ToArray());
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            updateUI();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartStop(true);
            updateUI();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StartStop(false);
            updateUI();
        }

        private void bRestore_Click(object sender, EventArgs e)
        {
            //restore game
            var file = (string) listView1.SelectedItems[0].Tag;
            if (string.IsNullOrEmpty(file) || !File.Exists(file) || string.IsNullOrEmpty(_appSettings.GameCatalog)) return;
            var fn = Path.GetFileName(file);
            var res = MessageBox.Show(string.Format("Are you really want to restore savegame:\n'{0}'?\n\nWARNING:Current save game will be replaced with this save!", fn), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (var zip = new Ionic.Zip.ZipFile(file))
                {
                    foreach (var f in zip)
                    {
                        f.Extract(_appSettings.GameCatalog, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Some error(s) was rased while we've tryed to restore archived save game:\n'{0}'.\nError message was:\n'{1}'", file, err.Message), "Huston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show(string.Format("We've successfully restored archived save game:\n'{0}'!", fn), "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int _startTimer;
        private void StartStop(bool start)
        {
            if (start)
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Saving...";
                saveGame();
                timer1.Interval = (int) _appSettings.SaveInterval.TotalMilliseconds;
                _startTimer = Environment.TickCount;
                timer1.Start();
                timer2.Start();
                toolStripProgressBar1.Maximum = (int) _appSettings.SaveInterval.TotalMilliseconds;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Wating";
                toolStripStatusLabel1.ForeColor = Color.Green;
                notifyIcon1.Icon = _iconList[1];
            }
            else
            {
                timer1.Stop();
                timer2.Stop();
                toolStripProgressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Stopped";
                toolStripStatusLabel1.ForeColor = Color.Black;
                toolStripStatusLabel4.Text = "";
                notifyIcon1.Icon = _iconList[0];
            }

        }

        private int _imageNo = (int)ImagesEnum.GameSaver9;
        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            notifyIcon1.Icon = _iconList[_imageNo--];
            if (_imageNo < (int) ImagesEnum.GameSaver1) _imageNo = (int) ImagesEnum.GameSaver9;
            var t = Environment.TickCount - _startTimer;
            toolStripProgressBar1.Value = t;
            toolStripStatusLabel4.Text = new TimeSpan(0,0,0,((int)_appSettings.SaveInterval.TotalMilliseconds - t)/1000).ToString("g");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _startTimer = Environment.TickCount;
            saveGame();
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            var file = (string)listView1.SelectedItems[0].Tag;
            if (File.Exists(file))
            {
                File.Delete(file);
                updateList();
                updateUI();
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            updateUI();
        }

        private void GameSaver_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }
    }
}
