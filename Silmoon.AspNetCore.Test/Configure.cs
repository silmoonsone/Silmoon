using Newtonsoft.Json.Linq;
using Silmoon.Extension;

namespace Silmoon.AspNetCore.Test
{
    public class Configure
    {
        public const string SigntrueKey = "yDHm6Vo1ktl5LDNQ";

        private static JObject configJson;
        public static JObject ConfigJson
        {
            get
            {
                if (configJson == null) configJson = GetJsonConfig();
                return configJson;
            }
        }
        static JObject GetJsonConfig()
        {
#if DEBUG
            var json = JsonHelper.LoadJsonFromFile(@"config.debug.json");
#else
            var json = JsonHelper.LoadJsonFromFile(@"config.json");
#endif
            return json;
        }
    }
}
