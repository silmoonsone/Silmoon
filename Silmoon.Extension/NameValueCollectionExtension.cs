using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Silmoon.Extension
{
    public static class NameValueCollectionExtension
    {
        public static string[] GetValues(this NameValueCollection nameValueCollection, string headerName)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < nameValueCollection.Count; i++)
            {
                if (nameValueCollection.GetKey(i) == headerName)
                {
                    result.Add(nameValueCollection[i]);
                }
            }
            return result.ToArray();
        }
        public static string ToQueryString(this NameValueCollection nameValueCollection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < nameValueCollection.Count; i++)
            {
                stringBuilder.Append(HttpUtility.UrlEncode(nameValueCollection.GetKey(i)) + "=" + HttpUtility.UrlEncode(nameValueCollection[i]));
            }
            return stringBuilder.ToString();
        }
    }
}
