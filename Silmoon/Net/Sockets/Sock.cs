using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Silmoon.Net.Sockets
{
    public class Sock
    {
        public static ushort getNetCheckSum(byte[] ipTcpHeader, int index, int offset)
        {
            uint sum = 0;

            int tindex = index + 20;
            offset = offset - 20;

            while (offset > 1)
            {
                sum += ntoh(BitConverter.ToUInt16(ipTcpHeader, tindex));
                tindex += 2;
                offset -= 2;
            }
            if (offset == 1) sum += ntoh(BitConverter.ToUInt16(new byte[] { ipTcpHeader[tindex], 0 }, 0));

            // Pseudo header - Source Address 
            sum += ntoh(BitConverter.ToUInt16(ipTcpHeader, 12 + index));
            sum += ntoh(BitConverter.ToUInt16(ipTcpHeader, 14 + index));
            // Pseudo header - Dest Address 
            sum += ntoh(BitConverter.ToUInt16(ipTcpHeader, 16 + index));
            sum += ntoh(BitConverter.ToUInt16(ipTcpHeader, 18 + index));
            // Pseudo header - Protocol 
            sum += ntoh(BitConverter.ToUInt16(new byte[] { 0, ipTcpHeader[9 + index] }, 0));
            // Pseudo header - TCP Header length 
            sum += (uint)(ipTcpHeader.Length - 20 - index);

            sum = ~((sum & 0xFFFF) + (sum >> 16));
            return ntoh((ushort)sum);
        }
        public static ushort getIpCheckSum(byte[] buffer, int index = 0, int offset = 20)
        {
            uint sum = 0;
            while (offset > 1)
            {
                sum += ntoh(BitConverter.ToUInt16(buffer, index));
                index += 2;
                offset -= 2;
            }
            if (offset == 1) sum += ntoh(BitConverter.ToUInt16(new byte[] { buffer[index - 1], 0 }, 0));


            sum = ~((sum & 0xFFFF) + (sum >> 16));
            return ntoh((ushort)sum);
        }
        private static ushort ntoh(UInt16 In)
        {
            int x = IPAddress.NetworkToHostOrder(In);
            return (ushort)(x >> 16);
        }
        public static byte[] makeInt16Data(UInt16 i)
        {
            byte[] b = BitConverter.GetBytes(i);
            Array.Reverse(b);
            return b;
        }
    }
}
