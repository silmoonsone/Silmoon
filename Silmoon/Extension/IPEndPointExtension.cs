using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Extension
{
    public static class IPEndPointExtension
    {
        public static IPEndPoint ToIPEndPoint(this string str)
        {
            string[] parts = str.Split(':');
            if (parts.Length != 2) throw new FormatException("Invalid IPEndPoint format");

            int port = int.Parse(parts[1]);
            if (port < 0 || port > 65535)
                throw new FormatException("Invalid IPEndPoint format");
            if (parts[0].IsIPAddress())
                return new IPEndPoint(IPAddress.Parse(parts[0]), port);
            else
                throw new FormatException("Invalid IPEndPoint format");
        }
        public static IPEndPoint[] ToIPEndPointArray(this string[] strs)
        {
            if (strs is null) return null;

            List<IPEndPoint> results = new List<IPEndPoint>();
            foreach (var item in strs)
            {
                if (item.IsNullOrEmpty()) continue;
                results.Add(item.ToIPEndPoint());
            }
            return results.ToArray();
        }
        public static string[] ToStringArray(this IPEndPoint[] IPEndPoints)
        {
            if (IPEndPoints is null) return null;

            List<string> results = new List<string>();
            foreach (var item in IPEndPoints)
            {
                if (item is null) continue;
                results.Add(item.ToString());
            }
            return results.ToArray();
        }
    }
}
