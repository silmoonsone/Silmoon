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
        public bool IgnoreCertificateValidation { get; set; } = false;
        public static JsonRequestSetting Default { get; } = new JsonRequestSetting();
        /// <summary>
        /// 在某些平台，比如Android MAUI，会造成请求HttpRequestMessage被Dispose的问题，设置为true可以解决这个问题
        /// </summary>
        public bool RequestMessageClone { get; set; } = false;

    }
}
