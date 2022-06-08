namespace GeneralGameSaver
{
    partial class Parameters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_CurrentDame = new System.Windows.Forms.ComboBox();
            this.btRemoveGame = new System.Windows.Forms.Button();
            this.tbGameName = new System.Windows.Forms.TextBox();
            this.btAddNewGame = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(671, 398);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 398);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 48);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cb_CurrentDame);
            this.groupBox1.Controls.Add(this.btRemoveGame);
            this.groupBox1.Controls.Add(this.tbGameName);
            this.groupBox1.Controls.Add(this.btAddNewGame);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 41);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Game Select";
            // 
            // cb_CurrentDame
            // 
            this.cb_CurrentDame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_CurrentDame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_CurrentDame.FormattingEnabled = true;
            this.cb_CurrentDame.Location = new System.Drawing.Point(6, 14);
            this.cb_CurrentDame.Name = "cb_CurrentDame";
            this.cb_CurrentDame.Size = new System.Drawing.Size(167, 21);
            this.cb_CurrentDame.TabIndex = 2;
            this.cb_CurrentDame.SelectedIndexChanged += new System.EventHandler(this.cb_CurrentDame_SelectedIndexChanged);
            // 
            // btRemoveGame
            // 
            this.btRemoveGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRemoveGame.Enabled = false;
            this.btRemoveGame.ForeColor = System.Drawing.Color.DarkRed;
            this.btRemoveGame.Location = new System.Drawing.Point(370, 12);
            this.btRemoveGame.Name = "btRemoveGame";
            this.btRemoveGame.Size = new System.Drawing.Size(57, 23);
            this.btRemoveGame.TabIndex = 5;
            this.btRemoveGame.Text = "Remove";
            this.btRemoveGame.UseVisualStyleBackColor = true;
            this.btRemoveGame.Click += new System.EventHandler(this.btRemoveGame_Click);
            // 
            // tbGameName
            // 
            this.tbGameName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbGameName.Location = new System.Drawing.Point(179, 14);
            this.tbGameName.Name = "tbGameName";
            this.tbGameName.Size = new System.Drawing.Size(142, 20);
            this.tbGameName.TabIndex = 4;
            this.tbGameName.TextChanged += new System.EventHandler(this.tbGameName_TextChanged);
            // 
            // btAddNewGame
            // 
            this.btAddNewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAddNewGame.Enabled = false;
            this.btAddNewGame.ForeColor = System.Drawing.Color.Green;
            this.btAddNewGame.Location = new System.Drawing.Point(327, 12);
            this.btAddNewGame.Name = "btAddNewGame";
            this.btAddNewGame.Size = new System.Drawing.Size(37, 23);
            this.btAddNewGame.TabIndex = 3;
            this.btAddNewGame.Text = "Add";
            this.btAddNewGame.UseVisualStyleBackColor = true;
            this.btAddNewGame.Click += new System.EventHandler(this.btAddNewGame_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(584, 16);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Location = new System.Drawing.Point(503, 16);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // Parameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 446);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Parameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parameters";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button btAddNewGame;
        private System.Windows.Forms.ComboBox cb_CurrentDame;
        private System.Windows.Forms.TextBox tbGameName;
        private System.Windows.Forms.Button btRemoveGame;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}