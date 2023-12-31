﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.Web.Extensions
{
    public static class HttpRequestExtension
    {
        public static NameValueCollection MergeHttpRequestCollection(this HttpRequestBase request)
        {
            NameValueCollection collection = request.QueryString;
            NameValueCollection collection2 = request.Form;
            foreach (var item in collection.AllKeys)
            {
                if (collection.AllKeys.Contains(item)) continue;
                else collection.Add(item, collection2[item]);
            }
            foreach (var item in collection2.AllKeys)
            {
                if (collection.AllKeys.Contains(item)) continue;
                else collection.Add(item, collection2[item]);
            }
            return collection;
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!(request["X-Requested-With"] == "XMLHttpRequest"))
            {
                if (request.Headers != null)
                {
                    return request.Headers["X-Requested-With"] == "XMLHttpRequest";
                }

                return false;
            }

            return true;
        }
    }
}
