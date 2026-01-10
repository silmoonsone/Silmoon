using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Net.Sockets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ArpHdr
    {
        /// <summary>
        /// 硬件类型
        /// </summary>
        [FieldOffset(0)]
        public ushort HardwardType;
        /// <summary>
        /// 协议类型
        /// </summary>
        [FieldOffset(2)]
        public ushort ProtocolType;
        /// <summary>
        /// 硬件地址长度
        /// </summary>
        [FieldOffset(4)]
        public byte HardwareSize;
        /// <summary>
        /// 协议地址长度
        /// </summary>
        [FieldOffset(5)]
        public byte ProtocolSize;
        /// <summary>
        /// 操作代码
        /// </summary>
        [FieldOffset(6)]
        public ushort OpCode;
        /// <summary>
        /// 发送者硬件地址
        /// </summary>
        [FieldOffset(8)]
        public MacAddress SenderMacAddress;
        /// <summary>
        /// 发送者协议地址
        /// </summary>
        [FieldOffset(14)]
        public uint SenderIPAddress;
        /// <summary>
        /// 目标硬件地址
        /// </summary>
        [FieldOffset(18)]
        public MacAddress TargetMacAddress;
        /// <summary>
        /// 目标协议地址
        /// </summary>
        [FieldOffset(24)]
        public uint TargetIPAddress;
    }
}
