using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon
{
    public static class Args
    {
        private static readonly Dictionary<string, string> _argsDictionary = new Dictionary<string, string>();
        public static string[] ArgsArray { get; set; }
        public static void ParseArgs(string[] args)
        {
            ArgsArray = args;
            string reservKeyName = null;
            foreach (var item in ArgsArray)
            {
                if (item[0] != '-' && reservKeyName != null)
                {
                    _argsDictionary[reservKeyName] = item;
                    reservKeyName = null;
                }
                else if (item[0] == '-')
                {
                    reservKeyName = null;
                    string[] aArray = item.Split(new char[] { '=' }, 2);
                    if (aArray.Length == 2)
                        _argsDictionary[aArray[0]] = aArray[1];
                    else
                    {
                        reservKeyName = aArray[0];
                        _argsDictionary[aArray[0]] = string.Empty;
                    }
                }
            }
        }
        public static string GetParameter(string key)
        {
            if (key.StartsWith("-"))
            {
                return _argsDictionary.FirstOrDefault(x => x.Key == key).Value;
            }
            else
            {
                foreach (var item in _argsDictionary)
                {
                    if (item.Key.StartsWith("--"))
                    {
                        if (item.Key.Substring(2) == key) return item.Value;
                    }
                    else if (item.Key.StartsWith("-"))
                    {
                        if (key.StartsWith(item.Key.Substring(1))) return item.Value;
                    }
                }
            }
            return _argsDictionary.TryGetValue(key, out var value) ? value : null;
        }

        public static IDictionary<string, string> GetAllParameters() => new Dictionary<string, string>(_argsDictionary);
    }
}
