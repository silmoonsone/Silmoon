using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Windows.Win32.API.APIEnum
{
    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive = 0,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }
}
