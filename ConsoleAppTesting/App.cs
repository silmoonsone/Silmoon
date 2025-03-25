using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTesting
{
    public class App : SilmoonDevApp, ISqlObject
    {
        public int id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid UserGuid { get; set; }
        public string AppPrivateKey { get; set; }
        public string DisplayName { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
