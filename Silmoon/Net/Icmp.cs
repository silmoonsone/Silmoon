using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;

namespace Silmoon.Net
{
    public class Icmp
    {
        private byte[] m_PingBuffer;
        public bool Ping(Uri address, int timeout = 3000) => address == null ? throw new ArgumentNullException("address") : Ping(address.Host, timeout);
        public bool Ping(string hostNameOrAddress, int timeout = 3000)
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) throw new InvalidOperationException("Network_NetworkNotAvailable");
            Ping ping = new Ping();
            return ping.Send(hostNameOrAddress, timeout, PingBuffer).Status == IPStatus.Success;
        }
        private byte[] PingBuffer
        {
            get
            {
                lock (m_PingBuffer)
                {
                    if (m_PingBuffer is null)
                    {
                        m_PingBuffer = new byte[32];
                        int index = 0;
                        do
                        {
                            m_PingBuffer[index] = Convert.ToByte(97 + (index % 23), CultureInfo.InvariantCulture);
                            index++;
                        }
                        while (index <= 31);
                    }
                    return m_PingBuffer;
                }
            }
        }
    }
}
