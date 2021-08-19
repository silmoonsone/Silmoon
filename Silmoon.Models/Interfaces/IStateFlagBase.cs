using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models.Interfaces
{
    public interface IStateFlagBase
    {
        string Message
        {
            get;
            set;
        }
        int StateCode
        {
            get;
            set;
        }
        bool Success
        {
            get;
            set;
        }
    }
}
