using Silmoon.Extension.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTesting
{
    public class SilmoonDevApp
    {
        public string AppId { get; set; }
        public string SignatureKey { get; set; }
        public string EncryptKey { get; set; }
        public Sex Level { get; set; }
        public UserGrade Status { get; set; }
    }
}
