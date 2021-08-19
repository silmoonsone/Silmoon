using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.Core.Extension
{
    public static class ControllerBaseExtension
    {
        public static JObject ReadToJson(this HttpRequest request)
        {
            request.Body.Position = 0;
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
            }
        }
    }
}
