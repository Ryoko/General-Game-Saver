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
    public partial class CommentInput : Form
    {
        public CommentInput(string comment)
        {
            InitializeComponent();
            textBox1.Text = comment;
        }

        public string Comment { get { return textBox1.Text; } }

        private void button1_Click(object sender, EventArgs e)
        {
//            DialogResult = DialogResult.OK;
        }

        private void CommentInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) 
                DialogResult = DialogResult.Cancel;
        }

        private void CommentInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n')
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
