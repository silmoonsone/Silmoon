using System;
using System.Collections.Generic;
using System.Net.Http;
using Silmoon.Runtime.Collections;

namespace Silmoon.Extension.Http
{
    public class JsonRequestSetting
    {
        public DictionaryEx<string, string[]> RequestHeaders { get; set; } = new DictionaryEx<string, string[]>();
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public static JsonRequestSetting Default { get; } = new JsonRequestSetting();
    }
}
