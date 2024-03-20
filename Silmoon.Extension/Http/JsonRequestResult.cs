using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension.Http
{
    public class JsonRequestResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Result { get; set; }
        public string Response { get; set; }
        public Exception Exception { get; set; }

        public JsonRequestResult(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }
        public JsonRequestResult(HttpStatusCode httpStatusCode, string response, Exception exception = null)
        {
            StatusCode = httpStatusCode;
            Response = response;
            Exception = exception;
            try
            {
                if (!response.IsNullOrEmpty()) Result = JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
        }

        public JsonRequestResult(Exception exception)
        {
            Exception = exception;
        }

        public bool IsSuccess => Exception == null && IsSuccessStatusCode;

        public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }
}
