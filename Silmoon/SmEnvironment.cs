using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Silmoon
{
    public class SmEnvironment
    {
        public static string GetAppRunPath()
        {
            string restring = "";
            if (Environment.GetCommandLineArgs()[0].LastIndexOf("\\") == -1)
            {
                restring = Environment.CurrentDirectory;
            }
            else
            {
                restring = Environment.GetCommandLineArgs()[0].Substring(0, Environment.GetCommandLineArgs()[0].LastIndexOf("\\"));

            }
            return restring;
        }
    }
}
