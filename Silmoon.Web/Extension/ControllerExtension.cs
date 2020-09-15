using Silmoon.Web.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.Extension
{
    public static class ControllerExtension
    {
        public static void SetCache(this System.Web.Mvc.ControllerBase controller, string Key, dynamic Value)
        {
            ObjectCache.Set(Key, Value);
        }

    }
}
