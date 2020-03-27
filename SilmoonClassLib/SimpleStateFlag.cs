using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon
{
    public class SimpleStateFlag
    {
        public int StateCode { get; set; } = 0;
        public string Message { get; set; } = "";
        public bool Success { get; set; } = false;
    }
}
