using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon.Instance
{
    public class VersionResult
    {
        public Exception Error;
        public int UserIdentityStateCode = -1;
        public string UserIdentityStateMessage = "#unknown";
        public int ExpiredVersion = -1;
        public int NotificationVersion = -1;
        public int LatestVersion = -1;

        public VersionResult()
        {

        }
    }
}
