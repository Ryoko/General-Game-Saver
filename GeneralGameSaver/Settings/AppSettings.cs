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
        [ReadOnlyAttribute(false), EditorAttribute(typeof(BrowseFolderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string GameCatalog { get => currentGameSettings.GameCatalog; set => currentGameSettings.GameCatalog = value; }

        [ReadOnlyAttribute(false), EditorAttribute(typeof(BrowseFolderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SaveCatalog { get => currentGameSettings.SaveCatalog; set => currentGameSettings.SaveCatalog = value; }

        public bool AutoStart { get => currentGameSettings.Autostart; set => currentGameSettings.Autostart = value; }

        public TimeSpan SaveInterval { get => currentGameSettings.Interval; set => currentGameSettings.Interval = value; }

        public int NumberOfFiles { get => currentGameSettings.NumberOfFiles; set => currentGameSettings.NumberOfFiles = value; }

        public AppSettings(GameSettings s)
        {
            //var s = Properties.Settings.Default;
            currentGameSettings = s;
        }

        public void PropertySave()
        {
            currentGameSettings.Save();
        }

        [Browsable(false)]
        public GameSettings currentGameSettings { get; set; }
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
