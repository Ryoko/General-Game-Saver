using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralGameSaver
{
    class ArchiveInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public string Group { get; set; }
    }
}
