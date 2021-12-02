using Silmoon.Arrays;
using Silmoon.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Silmoon.Extension
{
    public static class NameValueCollectionExtension
    {
        public static string[] GetValues(this NameValueCollection NameValueCollection, string KeyName)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                if (NameValueCollection.GetKey(i) == KeyName)
                {
                    result.Add(NameValueCollection[i]);
                }
            }
            return result.ToArray();
        }
        public static string ToQueryString(this NameValueCollection NameValueCollection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                stringBuilder.Append(HttpUtility.UrlEncode(NameValueCollection.GetKey(i)) + "=" + HttpUtility.UrlEncode(NameValueCollection[i]) + "&");
            }
            return stringBuilder.ToString().Remove(stringBuilder.Length - 1);
        }
        public static string GetSign(this NameValueCollection NameValueCollection, string KeyName, string KeyValue, string SignName = "sign")
        {
            ArrayList array = new ArrayList();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                if (NameValueCollection.GetKey(i).ToLower() == SignName) continue;
                array.Add(NameValueCollection.GetKey(i) + "=" + NameValueCollection[i]);
            }
            array.Sort(new DictionarySort());

            string s = "";

            foreach (var item in array) s += item + "&";

            s += KeyName + "=" + KeyValue;
            return HashHelper.MD5(s);
        }

    }
}
