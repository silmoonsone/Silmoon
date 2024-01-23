using Silmoon.Secure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                list.RemoveAll(item => item.Key == key);
                list.Add((key, value));
            }
        }

        public void Add(string key, object value)
        {
            if (value is string)
            {
                list.Add((key, value));
            }
            else if (value is IEnumerable items)
            {
                foreach (var item in items)
                {
                    list.Add((key, item?.ToString()));
                }
            }
            else
            {
                list.Add((key, value));
            }
        }
        [Obsolete]
        public void AddArray(string key, IEnumerable value)
        {
            if (value is IEnumerable items)
            {
                foreach (var item in items)
                {
                    list.Add((key, item?.ToString()));
                }
            }
        }
        public void Remove(string key) => list.RemoveAll(item => item.Key == key);

        public IEnumerator GetEnumerator() => list.GetEnumerator();

        public string GetSign(string AppendKey, string AppendValue, string IgnoreKey = "signature")
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
            s = s.TrimEnd('&');

            if (!AppendKey.IsNullOrEmpty() && !AppendValue.IsNullOrEmpty())
            {
                s += "&" + AppendKey + "=" + AppendValue;
            }
            else if (!AppendKey.IsNullOrEmpty() && AppendValue.IsNullOrEmpty())
            {
                s += AppendKey;
            }
            else if (AppendValue.IsNullOrEmpty() && !AppendKey.IsNullOrEmpty())
            {
                s += "&" + AppendValue;
            }
            return HashHelper.GetMD5Hash(s);
        }
        public byte[] GetSign(string AppendKey, string AppendValue, bool IgnoreEmptyValue, bool RequireValueUrlEncode, Func<string, byte[]> SignatureFunction, params string[] IgnoreKeys)
        {
            List<string> array = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (IgnoreKeys.Select(x => x.ToLower()).Contains(list[i].Key.ToLower())) continue;
                if (((string)list[i].Value).IsNullOrEmpty() && IgnoreEmptyValue) continue;
                array.Add(list[i].Key + "=" + (RequireValueUrlEncode ? HttpUtility.UrlEncode(list[i].Value.ToString()) : list[i].Value));
            }
            array.Sort();

            string s = "";

            foreach (var item in array) s += item + "&";
            s = s.TrimEnd('&');

            if (!AppendKey.IsNullOrEmpty() && !AppendValue.IsNullOrEmpty())
            {
                s += "&" + AppendKey + "=" + AppendValue;
            }
            else if (!AppendKey.IsNullOrEmpty() && AppendValue.IsNullOrEmpty())
            {
                s += AppendKey;
            }
            else if (AppendValue.IsNullOrEmpty() && !AppendKey.IsNullOrEmpty())
            {
                s += "&" + AppendValue;
            }


            var result = SignatureFunction(s);
            return result;
        }
        public string ToQueryString(bool UrlEncoded = true, bool Sort = false)
        {
            if (!Sort)
            {
                if (list.Count == 0) return default;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (UrlEncoded) stringBuilder.Append(HttpUtility.UrlEncode(list[i].Key) + "=" + HttpUtility.UrlEncode(list[i].Value?.ToString() ?? "") + "&");
                    else stringBuilder.Append(list[i].Key + "=" + list[i].Value?.ToString() ?? "" + "&");
                }
                return stringBuilder.ToString().Remove(stringBuilder.Length - 1);
            }
            else
            {
                List<string> keyList = new List<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (UrlEncoded) keyList.Add(HttpUtility.UrlEncode(list[i].Key));
                    else keyList.Add(list[i].Key);
                }
                keyList.Sort();

                List<string> array = new List<string>();
                foreach (var item in keyList)
                {
                    foreach (var item2 in list)
                    {
                        if (item2.Key == item)
                        {
                            if (UrlEncoded) array.Add(HttpUtility.UrlEncode(item2.Key) + "=" + HttpUtility.UrlEncode(item2.Value?.ToString() ?? ""));
                            else array.Add(item2.Key + "=" + item2.Value?.ToString() ?? "");
                        }
                    }
                }

                string s = "";

                foreach (var item in array) s += item + "&";
                return s.TrimEnd('&');
            }
        }
        public static UrlDataCollection Parse(string QueryString)
        {
            UrlDataCollection result = new UrlDataCollection();
            string[] qs = QueryString.Split('&');
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
        public override string ToString() => ToQueryString();

        public List<KeyValuePair<string, string>> GetKeyValuePairs()
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();

            foreach (var item in list)
            {
                keyValuePairs.Add(new KeyValuePair<string, string>(item.Key, item.Value.ToString()));
            }

            return keyValuePairs;
        }

        public static UrlDataCollection ParseUrl(string Url)
        {
            var uri = new Uri(Url);
            return Parse(uri.Query.TrimStart('?'));
        }
    }
}
