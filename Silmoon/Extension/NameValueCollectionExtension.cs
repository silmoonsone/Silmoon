using Silmoon.Arrays;
using Silmoon.Secure;
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
        public static string[] GetValues(this NameValueCollection NameValueCollection, string Key)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                if (NameValueCollection.GetKey(i) == Key)
                {
                    result.Add(NameValueCollection[i]);
                }
            }
            return result.ToArray();
        }
        public static string ToQueryString(this NameValueCollection NameValueCollection)
        {
            if (NameValueCollection is null || NameValueCollection.Count == 0) return "";
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                stringBuilder.Append(HttpUtility.UrlEncode(NameValueCollection.GetKey(i)) + "=" + HttpUtility.UrlEncode(NameValueCollection[i]) + "&");
            }
            return stringBuilder.ToString().Remove(stringBuilder.Length - 1);
        }
        public static NameValueCollection GetNameValueCollection(string Query)
        {
            NameValueCollection result = new NameValueCollection();
            string[] qs = Query.Split('&');
            foreach (var item in qs)
            {
                if (item.IsNullOrEmpty()) continue;
                string[] aqs = item.Split('=');
                if (aqs.Length == 2)
                {
                    result[aqs[0]] = HttpUtility.UrlDecode(aqs[1]);
                }
            }
            return result;
        }

        public static string GetSign(this NameValueCollection NameValueCollection, string KeyName, string Key, string IgnoreKey = "signature")
        {
            List<string> array = new List<string>();
            for (int i = 0; i < NameValueCollection.Count; i++)
            {
                if (NameValueCollection.GetKey(i).ToLower() == IgnoreKey.ToLower()) continue;
                array.Add(NameValueCollection.GetKey(i) + "=" + NameValueCollection[i]);
            }
            array.Sort();

            string s = "";

            foreach (var item in array) s += item + "&";

            s += KeyName + "=" + Key;
            return HashHelper.GetMD5Hash(s);
        }

    }
}
