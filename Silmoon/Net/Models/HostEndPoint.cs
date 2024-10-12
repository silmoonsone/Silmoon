using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Net.Models
{
    public class HostEndPoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public HostEndPoint(string host, int port)
        {
            Host = host;
            Port = port;
        }
        public IPAddress[] GetHostAddresses(bool ignoreDnsFail = false)
        {
            if (Host.IsIPAddress())
                return new IPAddress[] { IPAddress.Parse(Host) };
            else
            {
                if (ignoreDnsFail)
                {
                    try
                    {
                        return Dns.GetHostAddresses(Host);
                    }
                    catch (Exception)
                    {
                        return Array.Empty<IPAddress>();
                    }
                }
                else
                    return Dns.GetHostAddresses(Host);
            }
        }
        public IPEndPoint[] GetIPEndPoints(bool ignoreDnsFail = false)
        {
            IPAddress[] addresses = GetHostAddresses(ignoreDnsFail);
            List<IPEndPoint> results = new List<IPEndPoint>();
            foreach (var item in addresses)
            {
                results.Add(new IPEndPoint(item, Port));
            }
            return results.ToArray();
        }
        public static HostEndPoint Parse(string endPointString)
        {
            if (endPointString.IsNullOrEmpty()) throw new ArgumentException("str is null or empty");

            //str include ip6 addresss
            if (endPointString.Contains("]:"))
            {
                //ip6
                string[] parts = endPointString.Split(new string[] { "]:" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) throw new FormatException("Invalid HostEndPoint format");
                int port = int.Parse(parts[1]);
                if (port < 0 || port > 65535)
                    throw new FormatException("Invalid HostEndPoint format");
                return new HostEndPoint(parts[0].Substring(1), port);
            }
            else
            {
                //ip4
                string[] parts = endPointString.Split(':');
                if (parts.Length != 2) throw new FormatException("Invalid HostEndPoint format");
                int port = int.Parse(parts[1]);
                if (port < 0 || port > 65535)
                    throw new FormatException("Invalid HostEndPoint format");
                return new HostEndPoint(parts[0], port);
            }
        }
        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}
