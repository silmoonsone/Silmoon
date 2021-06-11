using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public interface IStateFlag
    {
        string Message
        {
            get;
            set;
        }
        int Code
        {
            get;
            set;
        }
        bool Success
        {
            get;
            set;
        }
        object UserState
        {
            get;
            set;
        }
    }
}
