using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data
{
    public class DbStateException : Exception
    {
        string message;
        DbStateFlag stateFlag;

        public DbStateFlag StateFlag
        {
            get { return stateFlag; }
            set { stateFlag = value; }
        }
        public override string Message
        {
            get
            {
                return message;
            }
        }

        public DbStateException(string message, DbStateFlag flag)
        {
            this.message = message;
            stateFlag = flag;
        }
    }
    public enum DbStateFlag
    {
        NotConnection=2,

    }
}
