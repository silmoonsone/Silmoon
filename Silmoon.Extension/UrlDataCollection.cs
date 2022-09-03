using Silmoon.Secure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Silmoon.Extension
{
    public class UrlDataCollection : IEnumerable
    {
        List<(string Key, object Value)> list = new List<(string, object)>();
        public UrlDataCollection()
        {

        }
        public int Count => list.Count;
        public void Add(string key, object value)
        {
            list.Add((key, value));
        }
        public void Remove(string key)
        {
            List<(string Key, object Value)> removeItems = new List<(string Key, object Value)>();
            foreach (var item in list)
            {
                if (item.Key == key)
                {
                    removeItems.Add(item);
                }
            }
            list.RemoveAll(removeItems.Contains);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public string GetSign(string KeyName, string Key, string IgnoreKey = "sign")
        {
            List<string> array = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Key.ToLower() == IgnoreKey.ToLower()) continue;
                array.Add(list[i].Key + "=" + list[i].Value);
            }
            array.Sort();

            string s = "";

            foreach (var item in array) s += item + "&";

            s += KeyName + "=" + Key;
            return HashHelper.MD5(s);
        }
        public string ToQueryString()
        {
            if (list.Count == 0) return "";
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(HttpUtility.UrlEncode(list[i].Key) + "=" + HttpUtility.UrlEncode(list[i].Value?.ToString() ?? "") + "&");
            }
            return stringBuilder.ToString().Remove(stringBuilder.Length - 1);
        }
        public static UrlDataCollection GetNameValueCollection(string Query)
        {
            UrlDataCollection result = new UrlDataCollection();
            string[] qs = Query.Split('&');
            foreach (var item in qs)
            {
                if (item.IsNullOrEmpty()) continue;
                string[] aqs = item.Split('=');
                if (aqs.Length == 2)
                {
                    result.Add(aqs[0], HttpUtility.UrlDecode(aqs[1]));
                }
            }
            return result;
        }
        public string this[string key]
        {
            get
            {
                string result = null;
                foreach (var item in list)
                {
                    if (item.Key == key)
                    {
                        if (result == null) result = "";
                        result += item.Value + ",";
                    }
                }
                if (result.EndsWith(",")) result = result.Remove(result.Length - 1);
                return result;
            }
            set
            {
                for (int i = 0; i < Count; i++)
                {
                    if (list[i].Key == key)
                    {
                        list[i] = (key, value);
                    }
                }
            }
        }
        public override string ToString() => ToQueryString();
    }
}
