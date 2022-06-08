using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralGameSaver
{
    public partial class Parameters : Form
    {

        private AppSettings appSettings;
        private AllGamesSettings allGamesSettings;

        public Parameters()
        {
            InitializeComponent();
            allGamesSettings = AllGamesSettings.LoadSettings();
            UpdateControls();
            CheckSettings();
        }

        private void UpdateControls()
        {
            cb_CurrentDame.Items.Clear();
            foreach (var key in allGamesSettings.GameKeys)
            {
                cb_CurrentDame.Items.Add(key);
            }

            if (!string.IsNullOrEmpty(allGamesSettings.CurrentGameKey) && allGamesSettings.ContainsKey(allGamesSettings.CurrentGameKey))
            {
                appSettings = new AppSettings(allGamesSettings.CurrentGameSettings);
                propertyGrid1.SelectedObject = appSettings;
                cb_CurrentDame.SelectedItem = allGamesSettings.CurrentGameKey;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }

            cb_CurrentDame.Enabled = allGamesSettings.GameKeys.Any();
            bOk.Enabled = propertyGrid1.SelectedObject != null;
        }

        private void CheckSettings()
        {
            if (!string.IsNullOrEmpty(allGamesSettings.CurrentGameKey) && allGamesSettings.ContainsKey(allGamesSettings.CurrentGameKey))
            {
                propertyGrid1.Enabled = true;
                btRemoveGame.Enabled = false;
            }
            else
            {
                propertyGrid1.Enabled = false;
            }

            if (propertyGrid1.SelectedObject != null)
            {
                var gameFolder = appSettings.GameCatalog;
                var backupFolder = appSettings.SaveCatalog;
                if (string.IsNullOrEmpty(gameFolder) || string.IsNullOrEmpty(backupFolder) || !Directory.Exists(gameFolder) || !Directory.Exists(backupFolder))
                {
                    bOk.Enabled = false;
                }
                else
                {
                    bOk.Enabled = true;
                }
            }

            btRemoveGame.Enabled = !string.IsNullOrEmpty((string)cb_CurrentDame.SelectedItem) && allGamesSettings.ContainsKey((string)cb_CurrentDame.SelectedItem);
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            appSettings.PropertySave();
        }

        private void btAddNewGame_Click(object sender, EventArgs e)
        {
            var newGameSettings = new GameSettings(allGamesSettings);
            appSettings = new AppSettings(newGameSettings);
            allGamesSettings[tbGameName.Text] = newGameSettings;
            allGamesSettings.CurrentGameKey = tbGameName.Text;
            var index = cb_CurrentDame.Items.Add(tbGameName.Text);
            cb_CurrentDame.SelectedIndex = index;
            tbGameName.Text = "";
            btAddNewGame.Enabled = false;
            CheckSettings();
        }

        private void tbGameName_TextChanged(object sender, EventArgs e)
        {
            btAddNewGame.Enabled = string.IsNullOrEmpty(tbGameName.Text) ? false : !allGamesSettings.ContainsKey(tbGameName.Text);
        }

        private void cb_CurrentDame_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedGame = (string)cb_CurrentDame.SelectedItem;
            if (allGamesSettings.ContainsKey(selectedGame))
            {
                allGamesSettings.CurrentGameKey = selectedGame;
                var gameSettings = allGamesSettings[selectedGame];
                appSettings = new AppSettings(gameSettings);
                propertyGrid1.SelectedObject = appSettings;
                propertyGrid1.Enabled = true;
            }
            CheckSettings();
        }

        private void btRemoveGame_Click(object sender, EventArgs e)
        {
            var selectedGame = cb_CurrentDame.SelectedItem as string;
            if (selectedGame != null && allGamesSettings.ContainsKey(selectedGame))
            {
                var confirm = MessageBox.Show($"Remove game settings for '{selectedGame}'", "Remove confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (confirm == DialogResult.OK)
                {
                    allGamesSettings.Remove(selectedGame);
                    allGamesSettings.CurrentGameKey = allGamesSettings.GameKeys.First();
                    UpdateControls();
                    CheckSettings();
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            CheckSettings();
        }
    }
}
