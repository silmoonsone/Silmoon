using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Silmoon.Runtime.Collections;

namespace Silmoon.Extension.Http
{
    public class JsonRequestSetting
    {
        public static JsonRequestSetting Default { get; } = new JsonRequestSetting();
        public DictionaryEx<string, string[]> RequestHeaders { get; set; } = new DictionaryEx<string, string[]>();
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = null;
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public bool IgnoreCertificateValidation { get; set; } = false;
    }
}
