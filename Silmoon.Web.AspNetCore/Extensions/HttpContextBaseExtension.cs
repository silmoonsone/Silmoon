using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.Web.AspNetCore.Extensions
{
    public static class HttpContextBaseExtension
    {
        public static NameValueCollection MergeHttpRequestCollection(this HttpContext httpContext)
        {
            var queries = httpContext.Request.Query;
            var forms = httpContext.Request.Form;

            NameValueCollection nameValueCollection = new NameValueCollection();

            foreach (var item in queries)
            {
                nameValueCollection.Add(item.Key, item.Value);
            }
            foreach (var item in forms)
            {
                nameValueCollection.Add(item.Key, item.Value);
            }

            return nameValueCollection;
        }
    }
}
