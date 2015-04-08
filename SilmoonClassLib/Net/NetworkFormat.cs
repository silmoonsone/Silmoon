using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Silmoon.Net
{
    /// <summary>
    /// 提供格式化网络地址的静态方法
    /// </summary>
    public class NetworkAddressFormat
    {
        /// <summary>
        /// 获取二进制的IP地址格式
        /// </summary>
        /// <param name="ip">原IP</param>
        /// <returns></returns>
        public static string IPv4ToBinaryAddress(IPAddress ip)
        {
            string[] ipstrArr = ip.ToString().Split(new string[] { "." }, StringSplitOptions.None);
            int cyccount = 0;
            string restring = "";
            foreach (string ipConStr in ipstrArr)
            {
                cyccount++;
                restring += Convert.ToString(Convert.ToInt32(ipConStr), 2);
                if (cyccount != 4) restring += ".";
            }
            return restring;
        }
        /// <summary>
        /// 获取子网掩码位数
        /// </summary>
        /// <param name="ip">IPv4格式的子网掩码</param>
        /// <returns></returns>
        public static int GetSubNetMaskCode(IPAddress ip)
        {
            return (int)((int)IPv4ToBinaryAddress(ip).Replace(".", "").LastIndexOf("1")) + 1;
        }
        public static IPAddress GetIPv4SubNetAddress(int submask)
        {
            string sstring = null;
            for (int i = 0; i < submask; i++)
            {
                sstring += "1";
            }
            for (int i = 0; i < 32 - submask; i++)
            {
                sstring += "0";
            }
            string ip1 = Convert.ToInt32(sstring.Substring(0, 8), 2).ToString();
            string ip2 = Convert.ToInt32(sstring.Substring(8, 8), 2).ToString();
            string ip3 = Convert.ToInt32(sstring.Substring(16, 8), 2).ToString();
            string ip4 = Convert.ToInt32(sstring.Substring(24, 8), 2).ToString();
            return IPAddress.Parse(ip1 + "." + ip2 + "." + ip3 + "." + ip4);
        }
        public static string GetIPPortString(IPEndPoint endPoint)
        {
            return endPoint.Address + ":" + endPoint.Port;
        }
        public static string GetIPPortString(TcpStruct tcpStruct)
        {
            return tcpStruct.IP + ":" + tcpStruct.Port;
        }
        public static uint GetNetmaskSubnetHostTotal(int mask)
        {
            return ~(0xffffffff << (int)(32 - mask));
        }
    }
}
