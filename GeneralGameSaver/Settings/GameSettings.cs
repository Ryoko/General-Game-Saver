using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeneralGameSaver
{
    [JsonObject]
    public class GameSettings
    {
        public GameSettings(AllGamesSettings allGamesSettings)
        {
            GameCatalog = "";
            SaveCatalog = "";
            Autostart = false;
            Interval = TimeSpan.FromMinutes(5);
            NumberOfFiles = 20;
            this.allGamesSettings = allGamesSettings;
        }

        public string GameCatalog { get; set; }

        public string SaveCatalog { get; set; }

        public bool Autostart { get; set; }

        public global::System.TimeSpan Interval { get; set; }

        public int NumberOfFiles { get; set; }

        [JsonIgnore]
        public AllGamesSettings allGamesSettings {get; set;}

        public void Save()
        {
            allGamesSettings?.SaveSettings();
        }
    }
}
