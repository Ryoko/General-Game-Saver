using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
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
        private AllGamesSettings allGamesSettings;
        private readonly List<Icon> _iconList;
        public GameSaver()
        {
            InitializeComponent(); 
            _iconList = new List<Icon>();
            loadIcons();
            allGamesSettings = AllGamesSettings.LoadSettings();
            _appSettings = new AppSettings(allGamesSettings.CurrentGameSettings);
            updateList();
            updateUI();
            if (IsSettingsReady())
            {
                StartStop(_appSettings.AutoStart);
            }
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
                allGamesSettings = AllGamesSettings.LoadSettings();
                _appSettings = new AppSettings(allGamesSettings.CurrentGameSettings);
                updateList(true);
                updateUI();
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            var isStarted = timer2.Enabled;
            StartStop(false);

            saveGame();

            if (isStarted)
                StartStop(true);
        }

        private void saveGame()
        {
            if (string.IsNullOrEmpty(_appSettings.SaveCatalog) || string.IsNullOrEmpty(_appSettings.GameCatalog))
                return;
            var dt = DateTime.MinValue;
            var path = _appSettings.GameCatalog;
            try
            {
                using (var zipfile = new Ionic.Zip.ZipFile())
                {
                    RecursiveArchivate(path, zipfile, "", ref dt);
                    dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                    if (dt <= _lastArchiveTime) return;
                    var gameCatalog = _appSettings.GameCatalog.TrimEnd(new[] {'\\'});
                    var name = Path.GetFileNameWithoutExtension(gameCatalog);
                    var filename = string.Format("{0}\\savegame_{1}_{2}.zip", _appSettings.SaveCatalog,
                        dt.ToString("yyMMdd-HHmmss"), name);
                    zipfile.Save(filename);
                    File.SetLastWriteTime(filename, dt);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(string.Format("Save game error: {0}", er.Message), "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            updateList();
            updateUI();
        }
        private static void RecursiveArchivate(string path, Ionic.Zip.ZipFile zip, string relPath, ref DateTime dt)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                //if (!CheckFile(file)) continue;
                try
                {
                    if (file.Contains("PendingOverwrite") || file.EndsWith(".lock"))
                    {
                        continue;
                    }
                    var fdt = File.GetLastWriteTime(file);
                    var attr = File.GetAccessControl(file);
                    if (fdt > dt) dt = File.GetLastWriteTime(file);
                    zip.AddFile(file, relPath);
                }catch(Exception err)
                {

                }
            }
            foreach (string directory in Directory.GetDirectories(path))
            {
                var dirName = new DirectoryInfo(directory).Name;
                RecursiveArchivate(directory, zip, string.Format(@"{0}\{1}", relPath, dirName), ref dt);
            }
        }

        private Dictionary<string, ArchiveInfo> GetArchives()
        {
 //           Shaman.DotNet archiveInfo
            var ci = Thread.CurrentThread.CurrentUICulture;
            var re = new Regex(@"^savegame_([\d-]+)_(.*)");
            var list = new Dictionary<string, ArchiveInfo>();
            var sf = _appSettings.SaveCatalog;
            if (string.IsNullOrEmpty(sf) || !Directory.Exists(sf)) return list;

            foreach (var file in Directory.GetFiles(sf))
            {
                var nm = Path.GetFileNameWithoutExtension(file);
                if (string.IsNullOrEmpty(nm)) continue;
                var comment = GetArchiveComment(file);
                var m = re.Match(nm);
                if (m.Success && m.Groups.Count > 2)
                {
                    try
                    {
                        var info = File.GetAttributes(file);
                        var dt = DateTime.ParseExact(m.Groups[1].Value, "yyMMdd-HHmmss", ci);
                        list[nm] = new ArchiveInfo()
                        {
                            Path = file, 
                            Date = dt, 
                            Group = m.Groups[2].Value, 
                            Locked = info.HasFlag(FileAttributes.ReadOnly), 
                            Name = nm,
                            Comment = comment
                        };
                    }catch{}
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
            bLock.Enabled = listView1.SelectedItems.Count > 0;
            toolStripStatusLabel1.Text = timer1.Enabled ? "Working..." : "Stopped";
            toolStripStatusLabel2.Text = _appSettings.GameCatalog;
            if (setReady) notifyIcon1.Icon = _iconList[0];
            cmStart.Enabled = !timer2.Enabled;
            cmStop.Enabled = timer2.Enabled;
            startToolStripMenuItem.Enabled = !timer2.Enabled;
            stopToolStripMenuItem.Enabled = timer2.Enabled;
            if (listView1.SelectedItems.Count > 0)
            {
                var ai = (ArchiveInfo) listView1.SelectedItems[0].Tag;
                bLock.Text = ai.Locked ? "Unlock" : "Lock";
                bRemove.Enabled = !ai.Locked;
            }
            else
            {
                bLock.Text = "Lock";
                bRemove.Enabled = false;
            }
        }

        private bool IsSettingsReady()
        {
            return (!string.IsNullOrEmpty(_appSettings.GameCatalog) && Directory.Exists(_appSettings.GameCatalog) &&
                    !string.IsNullOrEmpty(_appSettings.SaveCatalog) && Directory.Exists(_appSettings.SaveCatalog));
        }

        private void updateList(bool force = false)
        {
            var list = GetArchives();

            var purge = from ai in list
                where !ai.Value.Locked
                orderby ai.Value.Date descending
                group ai by ai.Value.Group
                into g
                select new {gr = g.Key, list = g.Skip(_appSettings.NumberOfFiles)};

            var lvItems = (from ai in list
                orderby ai.Value.Date descending
                group ai by ai.Value.Group
                into g
                select new LvGroup(g.Key, g.Where((r,n)=>r.Value.Locked || n < _appSettings.NumberOfFiles).Select(r=>r.Value).ToArray())).ToArray();

            var lastDt = list.Any() ? list.Max(r => r.Value.Date) : DateTime.MinValue;


            if (force || lastDt > _lastArchiveTime)
            {
                var grps = lvItems.Select(r => r.Grp).ToArray();
                var itms = lvItems.SelectMany(r => r.Items).ToArray();
                listView1.Items.Clear();
                listView1.Groups.AddRange(grps);
                listView1.Items.AddRange(itms);
                _lastArchiveTime = lastDt;
            }

            foreach (var gr in purge)
            {
                foreach (var f in gr.list)
                {
                    if (File.Exists(f.Value.Path))
                    {
                        File.Delete(f.Value.Path);
                    }
                }
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
            var isStarted = timer2.Enabled;
            StartStop(false);

            var archive = (ArchiveInfo)listView1.SelectedItems[0].Tag;
            var file = archive.Path;
            if (string.IsNullOrEmpty(file) || !File.Exists(file) || string.IsNullOrEmpty(_appSettings.GameCatalog)) return;
            var fn = Path.GetFileName(file);
            var res = MessageBox.Show(string.Format("Are you really want to restore savegame:\n'{0}'?\n\nWARNING:Current save game will be replaced with this save!", fn), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }


            using (var zip = new Ionic.Zip.ZipFile(file))
            {
                foreach (var f in zip)
                {
                    try
                    {
                        f.Extract(_appSettings.GameCatalog, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch (Exception err)
                    {
                        if (f.FileName.EndsWith("pregenData.json"))
                            continue;
                        MessageBox.Show($"Some error(s) was rased while we've tryed to restore archived save game:\n\r'{file}'\n\rError on restoring file:\n\r{f.FileName}\n\rError message:\n\r'{err.Message}'", "Huston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            MessageBox.Show(string.Format("We've successfully restored archived save game:\n'{0}'!", fn), "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (isStarted)
                StartStop(true);
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
            if (t > toolStripProgressBar1.Maximum) t = toolStripProgressBar1.Maximum;
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
            var archive = (ArchiveInfo)listView1.SelectedItems[0].Tag;
            var file = archive.Path;
            if (File.Exists(file) && !archive.Locked)
            {
                File.Delete(file);
                updateList(true);
                updateUI();
            }
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

        private void bLock_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var ai = (ArchiveInfo)listView1.SelectedItems[0].Tag;
                if (!ai.Locked)
                {
                    File.SetAttributes(ai.Path, FileAttributes.ReadOnly);
                }
                else
                {
                    File.SetAttributes(ai.Path, FileAttributes.Normal);
                }
                updateList(true);
            }
        }
        private class LvGroup
        {
            public ListViewGroup Grp { get; set; }
            public ListViewItem[] Items { get; set; }

            public LvGroup(string group, ArchiveInfo[] items)
            {
                Grp = new ListViewGroup(group);
                Items = items.Select(r => new ListViewItem(r.Name)
                {
                    Group = Grp,
                    Tag = r,
                    ImageIndex = r.Locked ? 1 : 0,
                    SubItems = { r.Date.ToString("g"), " " + r.Comment}
                }).ToArray();
            }
        }

        private string GetArchiveComment(string file)
        {
            try
            {
                using (var zip = new Ionic.Zip.ZipFile(file))
                {
                    return zip.Comment;
                }
            }
            catch (Exception err)
            {
                return "";
            }
        }

        private void SetArchiveComment(string file, string comment)
        {
            try
            {
                var isRO = false;
                if (File.GetAttributes(file) == FileAttributes.ReadOnly)
                {
                    isRO = true;
                    File.SetAttributes(file, FileAttributes.Normal);
                }
                using (var zip = new Ionic.Zip.ZipFile(file))
                {
                    zip.Comment = comment;
                    zip.Save();
                }
                if (isRO)
                    File.SetAttributes(file, FileAttributes.ReadOnly);
            }
            catch { }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 0) return;
            var archive = (ArchiveInfo) listView1.SelectedItems[0].Tag;
            var dlgComment = new CommentInput(archive.Comment);
            if (dlgComment.ShowDialog(this) == DialogResult.OK)
            {
                SetArchiveComment(archive.Path, dlgComment.Comment);
                updateList(true);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
    }
}
