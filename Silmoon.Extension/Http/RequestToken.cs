using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Http
{
    public class RequestToken
    {
        public string AppId { get; set; }
        public string SignKey { get; set; }
        public string EncryptKey { get; set; }
        public string Source { get; set; }
        public string CallerName { get; set; }
        private RequestToken()
        {

        }
        public static RequestToken Create(string AppId, string SignKey)
        {
            return Create(AppId, null, SignKey, null, null);
        }
        public static RequestToken Create(string AppId, string EncryptKey, string SignKey)
        {
            return Create(AppId, EncryptKey, SignKey, null, null);
        }
        public static RequestToken Create(string AppId, string EncryptKey, string SignKey, string Source, string CallerName)
        {
            return new RequestToken()
            {
                AppId = AppId,
                SignKey = SignKey,
                EncryptKey = EncryptKey,
                Source = Source,
                CallerName = CallerName
            };
        }
    }
}
