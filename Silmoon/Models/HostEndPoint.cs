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
            if (string.IsNullOrWhiteSpace(endPointString))
                throw new ArgumentException("Endpoint string is null or empty");

            // Handle IPv6 with port
            if (endPointString.Contains("]:"))
            {
                var parts = endPointString.Split(new[] { "]:" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    throw new FormatException("Invalid HostEndPoint format");

                if (!int.TryParse(parts[1], out int port) || port < 0 || port > 65535)
                    throw new FormatException("Invalid port");

                return new HostEndPoint(parts[0].TrimStart('['), port);
            }

            // Handle IPv4 or hostname with port
            var partsIPv4 = endPointString.Split(':');
            if (partsIPv4.Length != 2)
                throw new FormatException("Invalid HostEndPoint format");

            if (!int.TryParse(partsIPv4[1], out int portIPv4) || portIPv4 < 0 || portIPv4 > 65535)
                throw new FormatException("Invalid port");

            return new HostEndPoint(partsIPv4[0], portIPv4);
        }

        public override string ToString()
        {
            if (Host.IsIPv6Address())
                return $"[{Host}]:{Port}";
            else
                return $"{Host}:{Port}";
        }
        public override bool Equals(object obj)
        {
            // 先检查类型，如果不是 HostEndPoint，直接返回 false
            if (!(obj is HostEndPoint)) return false;

            // 强制转换 obj 为 HostEndPoint 后再比较
            var other = (HostEndPoint)obj;
            return Host.Equals(other.Host, StringComparison.OrdinalIgnoreCase) && Port == other.Port;
        }
        public override int GetHashCode()
        {
            return (Host?.GetHashCode() ?? 0) ^ Port.GetHashCode();
        }
        public static bool operator ==(HostEndPoint a, HostEndPoint b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(HostEndPoint a, HostEndPoint b)
        {
            return !(a == b);
        }
    }
}
