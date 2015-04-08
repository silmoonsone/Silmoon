using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Management;

namespace Silmoon.Windows.Systems
{
    public class Processes:IDisposable
    {
        ArrayList array = new ArrayList();
        Process[] sProcess = Process.GetProcesses();

        /// <summary>
        /// 获取进程数量
        /// </summary>
        public int ProcessCount
        {
            get { return 0; }
        }


        public void Refresh()
        {
            refreshProcess();
        }

        void refreshProcess()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Process");
            foreach (ManagementObject manObj in searcher.Get())
            {
                if (!IsExistInfoUnit(array, Convert.ToInt32(manObj["ProcessID"])))
                {
                    ProcessInfoUnit unitInfo = new ProcessInfoUnit();
                    unitInfo.ProcessID = int.Parse(manObj["ProcessID"].ToString());

                    if (manObj["Caption"] != null)
                        unitInfo.Caption = manObj["Caption"].ToString();

                    if (manObj["CommandLine"] != null) 
                        unitInfo.CommandLine = manObj["CommandLine"].ToString();

                    if (manObj["CreationDate"] != null)
                    {
                        string date = manObj["CreationDate"].ToString();
                        date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
                        unitInfo.CreationDate = DateTime.Parse(date);
                    }

                    array.Add(unitInfo);
                }
            }
            searcher.Dispose();
        }

        bool IsExistInfoUnit(object processInfoUnit, int PID)
        {
            foreach (ProcessInfoUnit s in (ArrayList)processInfoUnit)
            {
                if (s.ProcessID == PID)
                    return true;
            }
            return false;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            array.Clear();
        }

        #endregion
    }
    /// <summary>
    /// 进程信息单元
    /// </summary>
    public class ProcessInfoUnit
    {
        public string Name;
        public string Caption;
        public int ProcessID;
        public string CommandLine;
        public DateTime CreationDate;
        public PerformanceCounter _perf;
    }
}
