using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Enums
{
    /// <summary>
    /// 0为无状态，负数为错误，0以上为成功，并且带有状态
    /// </summary>
    public enum StateCode
    {
        NotExist = -5,
        ParameterError = -4,
        Failure = -3,
        Reject = -2,
        Error = -1,
        None = 0,
        Success = 1,
        PartalSuccess = 2,
    }
}
