using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.IO;

namespace Silmoon.Windows.Systems
{
    /// <summary>
    /// ϵͳ��Ϣ
    /// </summary>
    public class SystemInfo : IDisposable
    {
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref CPU_INFO cpuinfo);
        [DllImport("kernel32")]
        static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        //PerformanceCounter cpuTimePc;
        ManagementObjectSearcher searcher;


        /// <summary>
        /// ��ȡ�ڴ���Ϣ
        /// </summary>
        public MEMORY_INFO GetMemoryInfo
        {
            get
            {
                MEMORY_INFO meminfo = new MEMORY_INFO();
                GlobalMemoryStatus(ref meminfo);
                return meminfo;
            }
        }
        /// <summary>
        /// ����CPU��Ϣ
        /// </summary>
        public CPU_INFO GetCPUInfo
        {
            get
            {
                CPU_INFO cpuinfo = new CPU_INFO();
                GetSystemInfo(ref cpuinfo);
                return cpuinfo;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���CPUʱ��ٷֱ�����
        /// </summary>
        /// <returns></returns>
        public int[] CPUsLoadPercentage
        {
            get
            {
                if (File.Exists("/proc/stat")) return new int[0];

                ArrayList cpuLoadArr = new ArrayList();
                if (searcher == null) searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    cpuLoadArr.Add(int.Parse(queryObj["LoadPercentage"].ToString()));
                }
                return (int[])cpuLoadArr.ToArray(typeof(int));
            }
        }
        /// <summary>
        /// ��ȡ��ǰCPUʱ��ٷֱ�
        /// </summary>
        /// <returns></returns>
        public int CPULoadPercentage
        {
            get
            {
                int[] result = CPUsLoadPercentage;
                int c = result.Length;
                int d = 0;
                for (int i = 0; i < c; i++)
                {
                    d += result[i];
                }
                return (d / c);
            }
        }

        /// <summary>
        /// ��ȡϵͳ��ַ���λ��
        /// </summary>
        public static int SystemAddressWidth
        {
            get
            {
                ConnectionOptions oConn = new ConnectionOptions();
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn);
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oReturnCollection = oSearcher.Get();
                string addressWidth = null;

                foreach (ManagementObject oReturn in oReturnCollection)
                {
                    addressWidth = oReturn["AddressWidth"].ToString();
                }
                if (addressWidth == null) return 0;
                return int.Parse(addressWidth);
            }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            //if (cpuTimePc != null) cpuTimePc.Dispose();
            if (searcher != null) searcher.Dispose();
        }

        #endregion
    }
    /// <summary>
    /// CPU��Ϣ�ṹ
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CPU_INFO
    {
        public uint dwOemId;
        public uint dwPageSize;
        public uint lpMinimumApplicationAddress;
        public uint lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public uint dwProcessorLevel;
        public uint dwProcessorRevision;
    }
    /// <summary>
    /// �ڴ���Ϣ�ṹ
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }


}
