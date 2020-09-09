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
        public object Data { get; set; }
        public SimpleStateFlag()
        {

        }
        private SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            StateCode = stateCode;
            Message = message;
        }
        private SimpleStateFlag(bool success, int stateCode)
        {
            Success = success;
            StateCode = stateCode;
        }
        private SimpleStateFlag(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        private SimpleStateFlag(bool success)
        {
            Success = success;
        }
        public static SimpleStateFlag Create(bool success)
        {
            return new SimpleStateFlag(success);
        }
        public static SimpleStateFlag Create(bool success, string message)
        {
            return new SimpleStateFlag(success, message);
        }
        public static SimpleStateFlag Create(bool success, int stateCode)
        {
            return new SimpleStateFlag(success, stateCode);
        }
        public static SimpleStateFlag Create(bool success, int stateCode, string message)
        {
            return new SimpleStateFlag(success, stateCode, message);
        }
    }
}
