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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            label3.Text = $"v.{Application.ProductVersion}";
        }
    }
}
