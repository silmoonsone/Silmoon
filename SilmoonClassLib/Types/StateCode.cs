using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Types
{
    public enum StateCode
    {
        Failure = -3,
        Reject = -2,
        Error = -1,
        None = 0,
        Success = 1,
        PartalSuccess = 2,
    }
}
