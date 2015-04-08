using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.IO;
using Silmoon.Windows.Win32.API.APIStructs;
using Silmoon.Windows.Win32.API.APIEnum;

namespace Silmoon.Windows.Systems
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfo : IDisposable
    {
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref CPU_INFO cpuinfo);
        [DllImport("kernel32")]
        static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        PerformanceCounter cpuTimePc;
        ManagementObjectSearcher searcher;


        /// <summary>
        /// 获取内存信息
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
        /// 返回CPU信息
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
        /// 获取当前多个CPU时间百分比数组
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
        /// 获取当前CPU时间百分比
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
        /// 获取系统地址宽度位数
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

        #region IDisposable 成员

        public void Dispose()
        {
            //if (cpuTimePc != null) cpuTimePc.Dispose();
            if (searcher != null) searcher.Dispose();
        }

        #endregion
    }
    /// <summary>
    /// CPU信息结构
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
    /// 内存信息结构
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
