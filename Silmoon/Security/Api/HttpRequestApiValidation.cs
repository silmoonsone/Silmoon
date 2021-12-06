using Silmoon.Arrays;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silmoon.Security.Api
{
    public static class HttpRequestApiValidation
    {
        public static (string data, string sign) GetHttpRequestCollectionSign(NameValueCollection nameValueCollectionWithoutKey, string keyName, string key, params string[] excludedParams)
        {
            List<string> array = new List<string>();
            for (int i = 0; i < nameValueCollectionWithoutKey.Count; i++)
            {
                if (excludedParams.Contains(nameValueCollectionWithoutKey.GetKey(i))) continue;

                array.Add(nameValueCollectionWithoutKey.GetKey(i) + "=" + nameValueCollectionWithoutKey[i]);
            }
            array.Sort();

            string data = default;

            foreach (var item in array)
                data += item + "&";

            data += keyName + "=" + key;

            var sign = HashHelper.MD5(data);

            return (data, sign);
        }
        public static NameValueCollection MergeHttpRequestCollection(HttpRequestBase request)
        {
            NameValueCollection collection = request.QueryString;
            NameValueCollection collection2 = request.Form;
            foreach (var item in collection2.AllKeys)
            {
                if (collection.AllKeys.Contains(item)) continue;
                else collection.Add(item, collection2[item]);
            }
            return collection;
        }
    }
}
