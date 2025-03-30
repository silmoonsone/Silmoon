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
    public enum OperateResult
    {
        Exception = -5,
        Repeated = -4,
        ParameterMissing = -3,
        Rejected = -2,
        Error = -1,
        None = 0,
        Success = 1,
        Updated = 2,
    }
}
