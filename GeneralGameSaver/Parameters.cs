using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralGameSaver
{
    public partial class Parameters : Form
    {
        private AppSettings appSettings = new AppSettings();
        public Parameters()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = appSettings;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void bOk_Click(object sender, EventArgs e)
        {
            appSettings.PropertySave();
        }
    }
}
