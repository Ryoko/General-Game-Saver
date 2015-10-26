using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralGameSaver
{
    [DefaultProperty("SaveCatalog")]
    class AppSettings
    {
        private string _gameCatalog;
        private string _saveCatalog;
        private bool _autoStart;
        private TimeSpan _saveInterval;
        private int _numberOfFiles;

        [ReadOnlyAttribute(false), EditorAttribute(typeof(BrowseFolderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string GameCatalog
        {
            get { return _gameCatalog; } 
            set { _gameCatalog = value; }
        }

        [ReadOnlyAttribute(false), EditorAttribute(typeof(BrowseFolderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SaveCatalog 
        {
            get { return _saveCatalog; }
            set { _saveCatalog = value; }
        }

        public bool AutoStart
        {
            get { return _autoStart; }
            set { _autoStart = value; }
        }

        public TimeSpan SaveInterval
        {
            get { return _saveInterval; }
            set { _saveInterval = value; }
        }

        public int NumberOfFiles
        {
            get { return _numberOfFiles; }
            set { _numberOfFiles = value; }
        }

        public AppSettings()
        {
            var s = Properties.Settings.Default;
            _gameCatalog = s.GameCatalog;
            _saveCatalog = s.SaveCatalog;
            _autoStart = s.Autostart;
            _saveInterval = s.Interval;
            _numberOfFiles = s.NumberOfFiles;
        }

        public void PropertySave()
        {
            var s = Properties.Settings.Default;
            s.GameCatalog = _gameCatalog;
            s.SaveCatalog = _saveCatalog;
            s.Autostart = _autoStart;
            s.Interval = _saveInterval;
            s.NumberOfFiles = _numberOfFiles;
            s.Save();
        }
    }

    public class CatalogName : StringConverter
    {
        private string _name;
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
    }
    public class BrowseFolderEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }

            using (FolderBrowserDialog browseDialog = new FolderBrowserDialog())
            {
                if (value != null)
                {
                    browseDialog.SelectedPath = value.ToString();
                }

                browseDialog.Description = context.PropertyDescriptor.DisplayName;
                if (browseDialog.ShowDialog() == DialogResult.OK)
                {
                    value = browseDialog.SelectedPath;
                }
            }

            return value;
        }
    }
}
