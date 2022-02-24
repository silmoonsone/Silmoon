using Microsoft.AspNetCore.Mvc;
using Silmoon.Extension;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class ControllerBaseExtension
    {

        public static ContentResult JsonStateFlag(this ControllerBase controller, bool Success)
        {
            return JsonStateFlag(controller, Success, 0, null);
        }
        public static ContentResult JsonStateFlag(this ControllerBase controller, bool Success, string Message)
        {
            return JsonStateFlag(controller, Success, 0, Message);
        }
        public static ContentResult JsonStateFlag(this ControllerBase controller, bool Success, int Code)
        {
            return JsonStateFlag(controller, Success, Code, null);
        }
        public static ContentResult JsonStateFlag(this ControllerBase controller, bool Success, int Code, string Message)
        {
            var result = StateFlag.Create(Success, Code, Message);
            return new ContentResult() { Content = result.ToJsonString(), ContentType = "application/json" };
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success)
        {
            return JsonStateFlag<T>(controller, Success, 0, "", default);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, string Message)
        {
            return JsonStateFlag<T>(controller, Success, 0, Message, default);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code)
        {
            return JsonStateFlag<T>(controller, Success, Code, "", default);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, string Message)
        {
            return JsonStateFlag<T>(controller, Success, Code, Message, default);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, T Data)
        {
            return JsonStateFlag<T>(controller, Success, 0, "", Data);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, string Message, T Data)
        {
            return JsonStateFlag<T>(controller, Success, 0, Message, Data);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, T Data)
        {
            return JsonStateFlag<T>(controller, Success, Code, "", Data);
        }
        public static ContentResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, string Message, T Data)
        {
            var result = StateFlag<T>.Create(Success, Code, Message, Data);
            return new ContentResult() { Content = result.ToJsonString(), ContentType = "application/json" };
        }

    }
}
