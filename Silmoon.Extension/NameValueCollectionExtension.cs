using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Silmoon.Extension
{
    public static class NameValueCollectionExtension
    {
        public static string[] GetValues(this NameValueCollection nameValueCollection, string headerName)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < nameValueCollection.Count; i++)
            {
                if(nameValueCollection.GetKey(i) == headerName)
                {
                    result.Add(nameValueCollection[i]);
                }
            }
            return result.ToArray();
        }
    }
}
