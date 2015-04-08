using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace Silmoon.Configure
{
    public class NameValueReader
    {
        public static NameValueCollection ReadFromRawStringToNameValue(string rawString, string fieldSpliter, string nameValueSpliter)
        {
            NameValueCollection result = new NameValueCollection();
            string[] lines = rawString.Split(new string[] { fieldSpliter }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                string[] lineSplited = line.Split(new string[] { nameValueSpliter }, 2, StringSplitOptions.None);
                if (lineSplited.Length == 2)
                {
                    result.Add(lineSplited[0], lineSplited[1]);
                }
            }
            return result;
        }
        public static NameValueCollection ReadFromArrayStringToNameValue(string[] stringArray, string nameValueSpliter)
        {
            NameValueCollection result = new NameValueCollection();
            foreach (string line in stringArray)
            {
                string[] lineSplited = line.Split(new string[] { nameValueSpliter }, 2, StringSplitOptions.None);
                if (lineSplited.Length == 2)
                {
                    result.Add(lineSplited[0], lineSplited[1]);
                }
            }
            return result;
        }
    }
}