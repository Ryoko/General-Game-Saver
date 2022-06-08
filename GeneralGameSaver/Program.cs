using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralGameSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var configFile = "";
            if (args.Any())
            {
                var fn = args[0];
                if (File.Exists(fn))
                {
                    configFile = fn;
                }
                else
                {
                    
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameSaver());
        }
    }
}
