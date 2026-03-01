using Silmoon.Extensions.Models;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions
{
    public static class ExtensionMethods
    {
        public static StateResult ToStateResult(this StateFlag stateFlag) => new StateResult() { Success = stateFlag.Success, Code = stateFlag.Code, Message = stateFlag.Message };
        public static StateResult<T> ToStateResult<T>(this StateFlag<T> stateFlag) => new StateResult<T>() { Success = stateFlag.Success, Code = stateFlag.Code, Message = stateFlag.Message, Data = stateFlag.Data };
    }
}
