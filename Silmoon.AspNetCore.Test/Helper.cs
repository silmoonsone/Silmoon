using Newtonsoft.Json.Linq;
using Silmoon.Extension;

namespace Silmoon.AspNetCore.Test
{
    public class Helper
    {
        public static void Output(ILogger logger, string s, LogLevel logLevel = LogLevel.Information)
        {
            logger.Log(logLevel, s);
            Net.SocketHelper.UdpSendTo("[Silmoon.AspNetCore.XXX] " + s);
        }
    }
}
