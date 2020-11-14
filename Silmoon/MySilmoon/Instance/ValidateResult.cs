using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon.Instance
{
    public class VersionResult
    {
        public Exception Error;
        public int UserStateCode = -1;
        public string UserStateMessage = "#unknown";
        public int ExpiredVersion = -1;
        public int NotificationVersion = -1;
        public int LatestVersion = -1;
        public string Notice = "";
        public string UpgradeNotice = "";
        public string ExpiredNotice = "";

        public VersionResult()
        {

        }
    }
}
