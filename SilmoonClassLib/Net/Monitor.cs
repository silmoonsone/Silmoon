using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Silmoon.Net
{
    /// <summary>
    /// Monitor 的摘要说明。
    /// </summary>
    public class Monitor
    {
        public delegate void NewPacketEventHandler(Monitor m, Packet p);
        public event NewPacketEventHandler NewPacket;
        private Socket m_Monitor;
        private IPAddress m_Ip;
        private byte[] m_Buffer = new byte[65535];
        private const int IOC_VENDOR = 0x18000000;
        private const int IOC_IN = -2147483648;
        private const int SIO_RCVALL = IOC_IN ^ IOC_VENDOR ^ 1;
        private const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;
        private const int DOMAIN_ALIAS_RID_ADMINS = 0x220;

        public System.Net.IPAddress IP
        {
            get { return m_Ip; }
        }
        public byte[] Buffer
        {
            get { return m_Buffer; }
        }
        public Monitor()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public Monitor(IPAddress IpAddress)
        {
            if (!(Environment.OSVersion.Platform == PlatformID.Win32NT) && Environment.OSVersion.Version.Major < 5)
            {
                throw new NotSupportedException("This program requires Windows 2000, Windows XP or Windows .NET Server!");
            }
            m_Ip = IpAddress;
        }
        public void Start()
        {
            if (m_Monitor == null)
            {
                try
                {
                    m_Monitor = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                    m_Monitor.Bind(new IPEndPoint(IP, 0));
                    m_Monitor.IOControl(SIO_RCVALL, BitConverter.GetBytes(1), null);
                    //m_Monitor.BeginReceive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                catch (Exception e)
                {
                    //m_Monitor = null;
                    //throw new SocketException();
                }
                m_Monitor.BeginReceive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
        }
        public void Stop()
        {
            if (m_Monitor != null)
            {
                m_Monitor.Close();
            }
            m_Monitor = null;
        }
        public void OnReceive(System.IAsyncResult ar)
        {
            try
            {
                int received = m_Monitor.EndReceive(ar);
                try
                {
                    if (m_Monitor != null)
                    {
                        byte[] pkt = new byte[received];
                        Array.Copy(Buffer, 0, pkt, 0, received);
                        OnNewPacket(new Packet(pkt, DateTime.Now));
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                m_Monitor.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (Exception e)
            {

            }
        }

        protected void OnNewPacket(Packet p)
        {
            NewPacket(this, p);
        }
    }

    public enum Precedence
    {
        Routine = 0,
        Priority = 1,
        Immediate = 2,
        Flash = 3,
        FlashOverride = 4,
        CRITICECP = 5,
        InternetworkControl = 6,
        NetworkControl = 7
    }
    public enum Delay
    {
        NormalDelay = 0,
        LowDelay = 1
    }
    public enum Throughput
    {
        NormalThroughput = 0,
        HighThroughput = 1
    }
    public enum Reliability
    {
        NormalReliability = 0,
        HighReliability = 1
    }
    /// <summary>
    /// Packet 的摘要说明。
    /// </summary>
    public class Packet
    {
        private byte[] m_Raw;
        private DateTime m_Time;
        private int m_Version;
        private int m_HeaderLength;
        private Precedence m_Precedence;
        private Delay m_Delay;
        private Throughput m_Throughput;
        private Reliability m_Reliability;
        private int m_TotalLength;
        private int m_Identification;
        private int m_TimeToLive;
        private InternetProtocol m_Protocol;
        private byte[] m_Checksum;
        private string m_SourceAddress;
        private string m_DestinationAddress;
        private int m_SourcePort;
        private int m_DestinationPort;

        public Packet()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        //
        //   public Packet(byte[] raw):(byte[] raw, DateTime time

        //   {
        //    Packet(raw, DateTime.Now);
        //   }
        public Packet(byte[] raw, DateTime time)
        {
            if (raw == null)
            {
                throw new ArgumentNullException();
            }
            if (raw.Length < 20)
            {
                throw new ArgumentException();
            }

            this.m_Raw = raw;
            this.m_Time = time;
            this.m_HeaderLength = (raw[0] & 0xF) * 4;
            if ((raw[0] & 0xF) < 5) { throw new ArgumentException(); }
            this.m_Precedence = (Precedence)((raw[1] & 0xE0) >> 5);
            this.m_Delay = (Delay)((raw[1] & 0x10) >> 4);
            this.m_Throughput = (Throughput)((raw[1] & 0x8) >> 3);
            this.m_Reliability = (Reliability)((raw[1] & 0x4) >> 2);
            this.m_TotalLength = raw[2] * 256 + raw[3];
            if (!(this.m_TotalLength == raw.Length)) { throw new ArgumentException(); } // invalid size of packet;
            this.m_Identification = raw[4] * 256 + raw[5];
            this.m_TimeToLive = raw[8];

            m_Protocol = (InternetProtocol)raw[9];

            m_Checksum = new byte[2];
            m_Checksum[0] = raw[11];
            m_Checksum[1] = raw[10];

            try
            {
                m_SourceAddress = GetIPAddress(raw, 12);
                m_DestinationAddress = GetIPAddress(raw, 16);
            }
            catch (Exception e)
            {
                throw;
            }

            if (m_Protocol == InternetProtocol.Tcp || m_Protocol == InternetProtocol.Udp)
            {
                m_SourcePort = raw[m_HeaderLength] * 256 + raw[m_HeaderLength + 1];
                m_DestinationPort = raw[m_HeaderLength + 2] * 256 + raw[m_HeaderLength + 3];
            }
            else
            {

                m_SourcePort = -1;
                m_DestinationPort = -1;
            }
        }
        public string GetIPAddress(byte[] bArray, int nStart)
        {
            byte[] tmp = new byte[4];

            if (bArray.Length > nStart + 2)
            {
                tmp[0] = bArray[nStart];
                tmp[1] = bArray[nStart + 1];
                tmp[2] = bArray[nStart + 2];
                tmp[3] = bArray[nStart + 3];
            }

            return tmp[0] + "." + tmp[1] + "." + tmp[2] + "." + tmp[3];
        }
        public int TotalLength
        {
            get { return m_TotalLength; }
        }
        public DateTime Time
        {
            get
            {
                return this.m_Time;


            }
        }
        public InternetProtocol Protocol
        {
            get { return this.m_Protocol; }
        }
        public string SourceAddress
        {
            get { return this.m_SourceAddress; }
        }
        public string Source
        {
            get
            {
                if (m_SourcePort != -1)
                {
                    return SourceAddress.ToString() + ":" + m_SourcePort.ToString();
                }
                else
                {
                    return SourceAddress.ToString();
                }
            }
        }
        public string Destination
        {
            get
            {
                if (this.m_DestinationPort != -1)
                {
                    return DestinationAddress.ToString() + ":" + m_DestinationPort.ToString();
                }
                else
                {
                    return DestinationAddress.ToString();
                }
            }
        }
        public string DestinationAddress
        {

            get
            {
                return m_DestinationAddress;
            }
        }
    }
}
