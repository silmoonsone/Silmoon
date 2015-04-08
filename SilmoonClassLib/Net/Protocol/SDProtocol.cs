using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Silmoon.Net.Protocol
{
    public class SDProtocol
    {
        public SDProtocol()
        {

        }
        public SDPacket NewPacket(byte[] data)
        {
            return NewPacket(data, 0, 0);
        }
        public SDPacket NewPacket(byte[] data, int stateID)
        {
            return NewPacket(data, 0, stateID, 0);
        }
        public SDPacket NewPacket(byte[] data, SDFlags flags)
        {
            return NewPacket(data, 0, flags);
        }
        public SDPacket NewPacket(byte[] data, SDFlags flags, int stateID)
        {
            return NewPacket(data, 0, stateID, flags);
        }
        public SDPacket NewPacket(byte[] data, uint serviceID)
        {
            return NewPacket(data, 0, 0);
        }
        public SDPacket NewPacket(byte[] data, uint serviceID, SDFlags flags)
        {
            return NewPacket(data, serviceID, 0, flags);
        }
        public SDPacket NewPacket(byte[] data, uint serviceID, int stateID, SDFlags flags)
        {
            SDPacket packet = new SDPacket();
            packet.PacketID = 0;
            packet.ServiceID = serviceID;
            packet.Flags = flags;
            packet.StateID = stateID;
            if (data != null)
            {
                packet.Length = (uint)data.Length;
                packet.Data = data;
            }
            return packet;
        }

        public SDPacket? ReadPacket(byte[] rawData)
        {
            if (rawData == null) return null;
            if (rawData.Length < 20) return null;
            uint dataLen = BitConverter.ToUInt16(rawData, 16);
            if (rawData.Length < 20 + dataLen) return null;

            SDPacket packet = new SDPacket();
            packet.Data = new byte[dataLen];

            packet.PacketID = BitConverter.ToUInt16(rawData, 0);
            packet.ServiceID = BitConverter.ToUInt16(rawData, 4);
            packet.Flags = (SDFlags)BitConverter.ToUInt16(rawData, 8);
            packet.StateID = BitConverter.ToUInt16(rawData, 12);
            packet.Length = dataLen;
            Array.Copy(rawData, 20, packet.Data, 0, packet.Length);

            return packet;
        }

        public byte[] GetBytes(SDPacket packet)
        {
            byte[] data = new byte[20 + packet.Length];
            Array.Copy(BitConverter.GetBytes(packet.PacketID), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes(packet.ServiceID), 0, data, 4, 4);
            Array.Copy(BitConverter.GetBytes((uint)packet.Flags), 0, data, 8, 4);
            Array.Copy(BitConverter.GetBytes(packet.StateID), 0, data, 12, 4);
            Array.Copy(BitConverter.GetBytes(packet.Length), 0, data, 16, 4);
            Array.Copy(packet.Data, 0, data, 20, packet.Length);
            return data;
        }

        public byte[] GetPacketBytes(byte[] data)
        {
            return GetPacketBytes(data, 0, 0, 0);
        }
        public byte[] GetPacketBytes(byte[] data, int stateID)
        {
            return GetPacketBytes(data, 0, stateID, 0);
        }
        public byte[] GetPacketBytes(byte[] data, SDFlags flags)
        {
            return GetPacketBytes(data, 0, flags);
        }
        public byte[] GetPacketBytes(byte[] data, SDFlags flags, int stateID)
        {
            return GetPacketBytes(data, 0, stateID, flags);
        }
        public byte[] GetPacketBytes(byte[] data, uint serviceID)
        {
            return GetPacketBytes(data, serviceID, 0);
        }
        public byte[] GetPacketBytes(byte[] data, uint serviceID, SDFlags flags)
        {
            return GetPacketBytes(data, serviceID, 0, flags);
        }
        public byte[] GetPacketBytes(byte[] data, uint serviceID, int stateID, SDFlags flags)
        {
            byte[] result = new byte[20 + data.Length];
            Array.Copy(BitConverter.GetBytes(serviceID), 0, result, 4, 4);
            Array.Copy(BitConverter.GetBytes((int)flags), 0, result, 8, 4);
            Array.Copy(BitConverter.GetBytes(stateID), 0, result, 12, 4);
            if (data != null)
            {
                Array.Copy(BitConverter.GetBytes(data.Length), 0, result, 16, 4);
                Array.Copy(data, 0, result, 20, data.Length);
            }
            return result;
        }
        public SDPacket? FromSocketReceivePacket(Socket socket)
        {
            byte[] data = new byte[512];
            int len = 0;
            List<byte> dataList = new List<byte>();
            SDPacket? recvPacket = null;
            do
            {
                try
                {
                    len = socket.Receive(data);
                }
                catch
                {
                    return null;
                }
                for (int i = 0; i < len; i++) dataList.Add(data[i]);
                byte[] aData = dataList.ToArray();
                recvPacket = ReadPacket(aData);
            }
            while (!recvPacket.HasValue);

            return recvPacket.Value;
        }

        public SDPacket DeriveNewPacket(SDPacket packet)
        {
            SDPacket newPacket = new SDPacket();
            newPacket.PacketID = packet.PacketID;
            newPacket.ServiceID = packet.ServiceID;
            return newPacket;
        }
    }
}
