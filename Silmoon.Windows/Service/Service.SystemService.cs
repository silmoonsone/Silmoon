using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Net;
using Microsoft.Win32;
using System.Net.Sockets;
using System.Linq;

namespace Silmoon.Windows.Service.SystemService
{
    /// <summary>
    /// 系统服务环境
    /// </summary>
    public sealed class ServiceEnvironment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="protocol"></param>
        /// <param name="enable"></param>
        /// <param name="name"></param>
        public static void AddSharedAccessFirewallPort(int port, ProtocolType protocol, bool enable, string name)
        {
            string protocolStr = string.Empty;
            string enableStr = string.Empty;
            if (protocol == ProtocolType.Tcp) { protocolStr = "TCP"; }
            else if (protocol == ProtocolType.Udp) { protocolStr = "UDP"; }

            if (enable) { enableStr = "Enabled"; } else { enableStr = "Disabled"; }

            string portStr = port.ToString();


            RegistryKey k1 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile", true);
            if (!k1.GetSubKeyNames().Contains("GloballyOpenPorts")) k1.CreateSubKey("GloballyOpenPorts");
            k1.Close();

            RegistryKey k2 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\GloballyOpenPorts", true);
            if (!k2.GetSubKeyNames().Contains("List")) k2.CreateSubKey("List");
            k2.Close();

            RegistryKey k3 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\GloballyOpenPorts\List", true);
            k3.SetValue(portStr + ":" + protocolStr, portStr + ":" + protocolStr + ":*:" + enableStr + ":" + name, RegistryValueKind.String);
            k3.Close();
        }
    }
}