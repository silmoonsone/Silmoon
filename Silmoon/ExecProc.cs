using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Silmoon.SystemAppliaction
{
    public sealed class ExecProc
    {
        Process p = new Process();
        public ExecProc()
        {
            InitClass();
        }
        private void InitClass()
        {

        }

        public static void Run(string fileName, bool onSelfDir)
        {
            Process p = new Process();
            p.StartInfo.FileName = fileName;
            string tmpfm = fileName.Replace("/", "\\");
            int lc = tmpfm.LastIndexOf("\\");
            string name = tmpfm.Substring((tmpfm.Length - lc), tmpfm.Length);
        }
    }
}
