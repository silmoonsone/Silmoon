using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Silmoon.Net.NetworkInformation
{
    public class ConnectionsInformation
    {
        private const int NO_ERROR = 0;
        private const int MIB_TCP_STATE_CLOSED = 1;
        private const int MIB_TCP_STATE_LISTEN = 2;
        private const int MIB_TCP_STATE_SYN_SENT = 3;
        private const int MIB_TCP_STATE_SYN_RCVD = 4;
        private const int MIB_TCP_STATE_ESTAB = 5;
        private const int MIB_TCP_STATE_FIN_WAIT1 = 6;
        private const int MIB_TCP_STATE_FIN_WAIT2 = 7;
        private const int MIB_TCP_STATE_CLOSE_WAIT = 8;
        private const int MIB_TCP_STATE_CLOSING = 9;
        private const int MIB_TCP_STATE_LAST_ACK = 10;
        private const int MIB_TCP_STATE_TIME_WAIT = 11;
        private const int MIB_TCP_STATE_DELETE_TCB = 12;

        public TcpConnectInfo TcpConnexion;
        public TcpConnectStats TcpStats;
        public TcpConnectExInfo TcpExConnexions;

        public UdpConnectStats UdpStats;
        public UdpConnectInfo UdpConnexion;
        public UdpConnectExInfo UdpExConnexion;

        #region Tcp Function

        public TcpConnectStats GetTcpStats()
        {
            TcpStats = new TcpConnectStats();
            ConInfosHlpAPI32Wrapper.GetTcpStatistics(ref TcpStats);
            return TcpStats;
        }

        public TcpConnectExInfo GetExTcpConnexions()
        {
            // the size of the MIB_EXTCPROW struct =  6*DWORD
            int rowsize = 24;
            int BufferSize = 100000;
            // allocate a dumb memory space in order to retrieve  nb of connexion
            IntPtr lpTable = Marshal.AllocHGlobal(BufferSize);
            //getting infos
            int res = ConInfosHlpAPI32Wrapper.AllocateAndGetTcpExTableFromStack(ref lpTable, true, ConInfosHlpAPI32Wrapper.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                Debug.WriteLine("Erreur : " + ConInfosHlpAPI32Wrapper.GetAPIErrorMessageDescription(res) + " " + res);
                return TcpExConnexions; // Error. You should handle it
            }
            int CurrentIndex = 0;
            //get the number of entries in the table
            int NumEntries = (int)Marshal.ReadIntPtr(lpTable);
            lpTable = IntPtr.Zero;
            // free allocated space in memory
            Marshal.FreeHGlobal(lpTable);



            ///////////////////
            // calculate the real buffer size nb of entrie * size of the struct for each entrie(24) + the dwNumEntries
            BufferSize = (NumEntries * rowsize) + 4;
            // make the struct to hold the resullts
            TcpExConnexions = new TcpConnectExInfo();
            // Allocate memory
            lpTable = Marshal.AllocHGlobal(BufferSize);
            res = ConInfosHlpAPI32Wrapper.AllocateAndGetTcpExTableFromStack(ref lpTable, true, ConInfosHlpAPI32Wrapper.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                Debug.WriteLine("Erreur : " + ConInfosHlpAPI32Wrapper.GetAPIErrorMessageDescription(res) + " " + res);
                return TcpExConnexions; // Error. You should handle it
            }
            // New pointer of iterating throught the data
            IntPtr current = lpTable;
            CurrentIndex = 0;
            // get the (again) the number of entries
            NumEntries = (int)Marshal.ReadIntPtr(current);
            TcpExConnexions.dwNumEntries = NumEntries;
            // Make the array of entries
            TcpExConnexions.table = new TcpConnectExTable[NumEntries];
            // iterate the pointer of 4 (the size of the DWORD dwNumEntries)
            CurrentIndex += 4;
            current = (IntPtr)((int)current + CurrentIndex);
            // for each entries
            for (int i = 0; i < NumEntries; i++)
            {

                // The state of the connexion (in string)
                TcpExConnexions.table[i].StrgState = this.convert_state((int)Marshal.ReadIntPtr(current));
                // The state of the connexion (in ID)
                TcpExConnexions.table[i].iState = (int)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                // get the local address of the connexion
                UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                // get the local port of the connexion
                UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                // Store the local endpoint in the struct and convertthe port in decimal (ie convert_Port())
                TcpExConnexions.table[i].Local = new IPEndPoint(localAddr, (int)convert_Port(localPort));
                // get the remote address of the connexion
                UInt32 RemoteAddr = (UInt32)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                UInt32 RemotePort = 0;
                // if the remote address = 0 (0.0.0.0) the remote port is always 0
                // else get the remote port
                if (RemoteAddr != 0)
                {
                    RemotePort = (UInt32)Marshal.ReadIntPtr(current);
                    RemotePort = convert_Port(RemotePort);
                }
                current = (IntPtr)((int)current + 4);
                // store the remote endpoint in the struct  and convertthe port in decimal (ie convert_Port())
                TcpExConnexions.table[i].Remote = new IPEndPoint(RemoteAddr, (int)RemotePort);
                // store the process ID
                TcpExConnexions.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
                // Store and get the process name in the struct
                TcpExConnexions.table[i].ProcessName = this.get_process_name(TcpExConnexions.table[i].dwProcessId);
                current = (IntPtr)((int)current + 4);

            }
            // free the buffer
            Marshal.FreeHGlobal(lpTable);
            // re init the pointer
            current = IntPtr.Zero;
            return TcpExConnexions;
        }

        public TcpConnectInfo GetTcpConnexions()
        {
            byte[] buffer = new byte[20000]; // Start with 20.000 bytes left for information about tcp table
            int pdwSize = 20000;
            int res = ConInfosHlpAPI32Wrapper.GetTcpTable(buffer, out pdwSize, true);
            if (res != NO_ERROR)
            {
                buffer = new byte[pdwSize];
                res = ConInfosHlpAPI32Wrapper.GetTcpTable(buffer, out pdwSize, true);
                if (res != 0)
                    return TcpConnexion;     // Error. You should handle it
            }

            TcpConnexion = new TcpConnectInfo();

            int nOffset = 0;
            // number of entry in the
            TcpConnexion.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            TcpConnexion.table = new TcpConnectTable[TcpConnexion.dwNumEntries];

            for (int i = 0; i < TcpConnexion.dwNumEntries; i++)
            {
                // state
                int st = Convert.ToInt32(buffer[nOffset]);
                // state in string
                TcpConnexion.table[i].StrgState = convert_state(st);
                // state  by ID
                TcpConnexion.table[i].iState = st;
                nOffset += 4;
                // local address
                string LocalAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;
                //local port in decimal
                int LocalPort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                    (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);

                nOffset += 4;
                // store the remote endpoint
                TcpConnexion.table[i].Local = new IPEndPoint(IPAddress.Parse(LocalAdrr), LocalPort);

                // remote address
                string RemoteAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;
                // if the remote address = 0 (0.0.0.0) the remote port is always 0
                // else get the remote port in decimal
                int RemotePort;
                //
                if (RemoteAdrr == "0.0.0.0")
                {
                    RemotePort = 0;
                }
                else
                {
                    RemotePort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                        (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);
                }
                nOffset += 4;
                TcpConnexion.table[i].Remote = new IPEndPoint(IPAddress.Parse(RemoteAdrr), RemotePort);
            }
            return TcpConnexion;
        }

        #endregion
        #region Udp Functions

        public UdpConnectStats GetUdpStats()
        {

            UdpStats = new UdpConnectStats();
            ConInfosHlpAPI32Wrapper.GetUdpStatistics(ref UdpStats);
            return UdpStats;
        }

        public UdpConnectInfo GetUdpConnexions()
        {
            byte[] buffer = new byte[20000]; // Start with 20.000 bytes left for information about tcp table
            int pdwSize = 20000;
            int res = ConInfosHlpAPI32Wrapper.GetUdpTable(buffer, out pdwSize, true);
            if (res != NO_ERROR)
            {
                buffer = new byte[pdwSize];
                res = ConInfosHlpAPI32Wrapper.GetUdpTable(buffer, out pdwSize, true);
                if (res != 0)
                    return UdpConnexion;     // Error. You should handle it
            }

            UdpConnexion = new UdpConnectInfo();

            int nOffset = 0;
            // number of entry in the
            UdpConnexion.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            UdpConnexion.table = new UdpConnectTable[UdpConnexion.dwNumEntries];
            for (int i = 0; i < UdpConnexion.dwNumEntries; i++)
            {
                string LocalAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;

                int LocalPort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                    (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);
                nOffset += 4;
                UdpConnexion.table[i].Local = new IPEndPoint(IPAddress.Parse(LocalAdrr), LocalPort);
            }
            return UdpConnexion;
        }

        public UdpConnectExInfo GetExUdpConnexions()
        {

            // the size of the MIB_EXTCPROW struct =  4*DWORD
            int rowsize = 12;
            int BufferSize = 100000;
            // allocate a dumb memory space in order to retrieve  nb of connexion
            IntPtr lpTable = Marshal.AllocHGlobal(BufferSize);
            //getting infos
            int res = ConInfosHlpAPI32Wrapper.AllocateAndGetUdpExTableFromStack(ref lpTable, true, ConInfosHlpAPI32Wrapper.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                Debug.WriteLine("Erreur : " + ConInfosHlpAPI32Wrapper.GetAPIErrorMessageDescription(res) + " " + res);
                return UdpExConnexion; // Error. You should handle it
            }
            int CurrentIndex = 0;
            //get the number of entries in the table
            int NumEntries = (int)Marshal.ReadIntPtr(lpTable);
            lpTable = IntPtr.Zero;
            // free allocated space in memory
            Marshal.FreeHGlobal(lpTable);

            ///////////////////
            // calculate the real buffer size nb of entrie * size of the struct for each entrie(24) + the dwNumEntries
            BufferSize = (NumEntries * rowsize) + 4;
            // make the struct to hold the resullts
            UdpExConnexion = new UdpConnectExInfo();
            // Allocate memory
            lpTable = Marshal.AllocHGlobal(BufferSize);
            res = ConInfosHlpAPI32Wrapper.AllocateAndGetUdpExTableFromStack(ref lpTable, true, ConInfosHlpAPI32Wrapper.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                Debug.WriteLine("Erreur : " + ConInfosHlpAPI32Wrapper.GetAPIErrorMessageDescription(res) + " " + res);
                return UdpExConnexion; // Error. You should handle it
            }
            // New pointer of iterating throught the data
            IntPtr current = lpTable;
            CurrentIndex = 0;
            // get the (again) the number of entries
            NumEntries = (int)Marshal.ReadIntPtr(current);
            UdpExConnexion.dwNumEntries = NumEntries;
            // Make the array of entries
            UdpExConnexion.table = new UdpConnectExTable[NumEntries];
            // iterate the pointer of 4 (the size of the DWORD dwNumEntries)
            CurrentIndex += 4;
            current = (IntPtr)((int)current + CurrentIndex);
            // for each entries
            for (int i = 0; i < NumEntries; i++)
            {
                // get the local address of the connexion
                UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                // get the local port of the connexion
                UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);
                // iterate the pointer of 4
                current = (IntPtr)((int)current + 4);
                // Store the local endpoint in the struct and convertthe port in decimal (ie convert_Port())
                UdpExConnexion.table[i].Local = new IPEndPoint(localAddr, convert_Port(localPort));
                // store the process ID
                UdpExConnexion.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
                // Store and get the process name in the struct
                UdpExConnexion.table[i].ProcessName = this.get_process_name(UdpExConnexion.table[i].dwProcessId);
                current = (IntPtr)((int)current + 4);

            }
            // free the buffer
            Marshal.FreeHGlobal(lpTable);
            // re init the pointer
            current = IntPtr.Zero;
            return UdpExConnexion;
        }

        #endregion
        #region helper fct

        private UInt16 convert_Port(UInt32 dwPort)
        {
            byte[] b = new Byte[2];
            // high weight byte
            b[0] = byte.Parse((dwPort >> 8).ToString());
            // low weight byte
            b[1] = byte.Parse((dwPort & 0xFF).ToString());
            return BitConverter.ToUInt16(b, 0);
        }
        private string convert_state(int state)
        {
            string strg_state = "";
            switch (state)
            {
                case MIB_TCP_STATE_CLOSED: strg_state = "CLOSED"; break;
                case MIB_TCP_STATE_LISTEN: strg_state = "LISTEN"; break;
                case MIB_TCP_STATE_SYN_SENT: strg_state = "SYN_SENT"; break;
                case MIB_TCP_STATE_SYN_RCVD: strg_state = "SYN_RCVD"; break;
                case MIB_TCP_STATE_ESTAB: strg_state = "ESTAB"; break;
                case MIB_TCP_STATE_FIN_WAIT1: strg_state = "FIN_WAIT1"; break;
                case MIB_TCP_STATE_FIN_WAIT2: strg_state = "FIN_WAIT2"; break;
                case MIB_TCP_STATE_CLOSE_WAIT: strg_state = "CLOSE_WAIT"; break;
                case MIB_TCP_STATE_CLOSING: strg_state = "CLOSING"; break;
                case MIB_TCP_STATE_LAST_ACK: strg_state = "LAST_ACK"; break;
                case MIB_TCP_STATE_TIME_WAIT: strg_state = "TIME_WAIT"; break;
                case MIB_TCP_STATE_DELETE_TCB: strg_state = "DELETE_TCB"; break;
            }
            return strg_state;
        }
        private string get_process_name(int processID)
        {
            //could be an error here if the process die before we can get his name
            try
            {
                Process p = Process.GetProcessById((int)processID);
                return p.ProcessName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "unknown";
            }

        }

        #endregion
    }
    public class ConInfosHlpAPI32Wrapper
    {
        public const byte NO_ERROR = 0;
        public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        public int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetUdpStatistics(ref UdpConnectStats pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int GetUdpTable(byte[] UcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetTcpStatistics(ref TcpConnectStats pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int AllocateAndGetTcpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int AllocateAndGetUdpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr GetProcessHeap();

        [DllImport("kernel32", SetLastError = true)]
        private static extern int FormatMessage(int flags, IntPtr source, int messageId, int languageId, StringBuilder buffer, int size, IntPtr arguments);

        public static string GetAPIErrorMessageDescription(int ApiErrNumber)
        {
            System.Text.StringBuilder sError = new System.Text.StringBuilder(512);
            int lErrorMessageLength;
            lErrorMessageLength = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, (IntPtr)0, ApiErrNumber, 0, sError, sError.Capacity, (IntPtr)0);

            if (lErrorMessageLength > 0)
            {
                string strgError = sError.ToString();
                strgError = strgError.Substring(0, strgError.Length - 2);
                return strgError + " (" + ApiErrNumber.ToString() + ")";
            }
            return "none";

        }
    }

    #region UDP
    [StructLayout(LayoutKind.Sequential)]
    public struct UdpConnectStats
    {
        public int dwInDatagrams;
        public int dwNoPorts;
        public int dwInErrors;
        public int dwOutDatagrams;
        public int dwNumAddrs;
    }
    public struct UdpConnectInfo
    {
        public int dwNumEntries;
        public UdpConnectTable[] table;

    }
    public struct UdpConnectTable
    {
        public IPEndPoint Local;
    }
    public struct UdpConnectExInfo
    {
        public int dwNumEntries;
        public UdpConnectExTable[] table;

    }
    public struct UdpConnectExTable
    {
        public IPEndPoint Local;
        public int dwProcessId;
        public string ProcessName;
    }

    #endregion
    #region TCP
    [StructLayout(LayoutKind.Sequential)]
    public struct TcpConnectStats
    {
        public int dwRtoAlgorithm;
        public int dwRtoMin;
        public int dwRtoMax;
        public int dwMaxConn;
        public int dwActiveOpens;
        public int dwPassiveOpens;
        public int dwAttemptFails;
        public int dwEstabResets;
        public int dwCurrEstab;
        public int dwInSegs;
        public int dwOutSegs;
        public int dwRetransSegs;
        public int dwInErrs;
        public int dwOutRsts;
        public int dwNumConns;
    }
    public struct TcpConnectInfo
    {
        public int dwNumEntries;
        public TcpConnectTable[] table;

    }
    public struct TcpConnectTable
    {
        public string StrgState;
        public int iState;
        public IPEndPoint Local;
        public IPEndPoint Remote;
    }
    public struct TcpConnectExInfo
    {
        public int dwNumEntries;
        public TcpConnectExTable[] table;

    }
    public struct TcpConnectExTable
    {
        public string StrgState;
        public int iState;
        public IPEndPoint Local;
        public IPEndPoint Remote;
        public int dwProcessId;
        public string ProcessName;
    }
    #endregion
}
