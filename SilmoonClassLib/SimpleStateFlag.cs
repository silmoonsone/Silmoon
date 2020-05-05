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
        public SimpleStateFlag()
        {

        }
        public SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            StateCode = stateCode;
            Message = message;
        }
        public SimpleStateFlag(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public SimpleStateFlag(bool success)
        {
            Success = success;
        }
    }
}
