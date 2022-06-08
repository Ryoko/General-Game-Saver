using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace GeneralGameSaver
{
    [JsonObject]
    public class AllGamesSettings
    {
        private const string SettingsFileName = "GameServerSettings.json";
        private static string filePath = $"{Application.StartupPath}\\{SettingsFileName}";
        [JsonRequired]
        private Dictionary<string, GameSettings> _gamesSettings;
        public string CurrentGameKey { get; set; }
        [JsonIgnore]
        public IEnumerable<string> GameKeys => _gamesSettings.Keys;

        public static AllGamesSettings LoadSettings()
        {
            AllGamesSettings allGamesSettings;
            try
            {
                if (File.Exists(filePath))
                {
                    var fileText = File.ReadAllText(filePath);
                    allGamesSettings = JsonConvert.DeserializeObject<AllGamesSettings>(fileText);
                }
                else
                {
                    allGamesSettings = new AllGamesSettings("", new Dictionary<string, GameSettings>());
                }
            }
            catch(Exception er)
            {
                allGamesSettings = new AllGamesSettings("", new Dictionary<string, GameSettings>());
            }
            foreach (var key in allGamesSettings.GameKeys)
            {
                allGamesSettings[key].allGamesSettings = allGamesSettings;
            }
            return allGamesSettings;
        }

        [JsonConstructor]
        private AllGamesSettings(string currentGameKey, Dictionary<string, GameSettings> gameSettingsDictionary)
        {
            this._gamesSettings = gameSettingsDictionary;
            this.CurrentGameKey = currentGameKey;
        }

        public void SaveSettings()
        {
            if (_gamesSettings.ContainsKey("")) _gamesSettings.Remove("");
            //Properties.Settings.Default.Setting = JsonConvert.SerializeObject(_gamesSettings);
            //Properties.Settings.Default.DefaultGame = CurrentGameKey;
            //Properties.Settings.Default.Save();
            var fileText = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, fileText);
        }

        [JsonIgnore]
        public GameSettings this[string gameName]
        {
            get => _gamesSettings.ContainsKey(gameName) ? _gamesSettings[gameName] : null;
            set => _gamesSettings[gameName] = value;
        }

        public bool ContainsKey(string gameName)
        {
            return _gamesSettings.ContainsKey(gameName);
        }

        public void Remove(string removeGameKey)
        {
            if (!string.IsNullOrEmpty(removeGameKey) && _gamesSettings.ContainsKey(removeGameKey))
            {
                _gamesSettings.Remove(removeGameKey);
            }
        }

        [JsonIgnore]
        public GameSettings CurrentGameSettings { 
            get 
            {
                if (!string.IsNullOrEmpty(CurrentGameKey) && _gamesSettings.ContainsKey(CurrentGameKey)) return _gamesSettings[CurrentGameKey];
                var newGameSettings = new GameSettings(this);
                _gamesSettings[CurrentGameKey] = newGameSettings;
                return newGameSettings;
            }
        }
    }
}
