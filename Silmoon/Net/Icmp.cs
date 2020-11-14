using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Globalization;

namespace Silmoon.Net
{
    public class Icmp
    {
        private byte[] m_PingBuffer;
        public bool Ping(string hostNameOrAddress)
        {
            return this.Ping(hostNameOrAddress, 1000);
        }
        public bool Ping(Uri address)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            return this.Ping(address.Host, 0x3e8);
        }
        public bool Ping(string hostNameOrAddress, int timeout)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new InvalidOperationException("Network_NetworkNotAvailable");
            Ping ping = new Ping();
            return (ping.Send(hostNameOrAddress, timeout, this.PingBuffer).Status == IPStatus.Success);
        }
        public bool Ping(Uri address, int timeout)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            return this.Ping(address.Host, timeout);
        }
        private byte[] PingBuffer
        {
            get
            {
                if (this.m_PingBuffer == null)
                {
                    this.m_PingBuffer = new byte[32];
                    int index = 0;
                    do
                    {
                        this.m_PingBuffer[index] = Convert.ToByte(97 + (index % 23), CultureInfo.InvariantCulture);
                        index++;
                    }
                    while (index <= 31);
                }
                return this.m_PingBuffer;
            }
        }
    }
}
