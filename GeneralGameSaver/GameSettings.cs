using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeneralGameSaver
{
    public class GameSettings : ISerializable
    {
        private static GameSettings defaultInstance = new GameSettings();

        public static GameSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        public GameSettings()
        {
            GameCatalog = "";
            SaveCatalog = "";
            Autostart = false;
            Interval = TimeSpan.FromMinutes(5);
        }

        private string _gameSettings;

        //[global::System.Configuration.UserScopedSettingAttribute()]
        //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //[global::System.Configuration.DefaultSettingValueAttribute("")]
        public string GameCatalog { get => _gameSettings; set => _gameSettings = value; }

        public string SaveCatalog { get; set; }

        public bool Autostart { get; set; }

        public global::System.TimeSpan Interval { get; set; }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class AllGamesSettings : ISerializable
    {
        private Dictionary<string, GameSettings> _gamesSettings;
        public AllGamesSettings()
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
