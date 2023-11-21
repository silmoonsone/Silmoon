using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Core.Models
{
    public class KeyFile
    {
        public string Name { get; set; }
        public string HashId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
