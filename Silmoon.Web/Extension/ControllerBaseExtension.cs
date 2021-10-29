using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silmoon.Web.Extension
{
    public static class ControllerBaseExtension
    {

        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, 0, "", null, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, string Message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, 0, Message, null, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, int Code, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, Code, "", null, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, int Code, string Message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, Code, Message, null, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, object Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, 0, "", Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, string Message, object Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, 0, Message, Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, int Code, object Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag(controller, Success, Code, "", Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag(this ControllerBase controller, bool Success, int Code, string Message, object Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonResult();
            result.Data = StateFlag.Create(Success, Code, Message, Data);
            result.JsonRequestBehavior = jsonRequestBehavior;
            result.ContentType = "application/json";
            result.ContentEncoding = Encoding.UTF8;
            return result;
        }

        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, 0, "", default, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, string Message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, 0, Message, default, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, Code, "", default, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, string Message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, Code, Message, default, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, T Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, 0, "", Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, string Message, T Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, 0, Message, Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, T Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return JsonStateFlag<T>(controller, Success, Code, "", Data, jsonRequestBehavior);
        }
        public static JsonResult JsonStateFlag<T>(this ControllerBase controller, bool Success, int Code, string Message, T Data, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonResult();
            result.Data = StateFlag<T>.Create(Success, Code, Message, Data);
            result.JsonRequestBehavior = jsonRequestBehavior;
            result.ContentType = "application/json";
            result.ContentEncoding = Encoding.UTF8;
            return result;
        }

    }
}
