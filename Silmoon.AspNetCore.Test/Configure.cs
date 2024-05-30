using Newtonsoft.Json.Linq;
using Silmoon.Extension;

namespace Silmoon.AspNetCore.Test;

public class Configure
{
    public static string ProjectName { get; set; }

    private static JObject configJson;
    public static JObject ConfigJson => configJson ??= GetJsonConfig();
    static JObject GetJsonConfig()
    {
        var json = JsonHelperV2.LoadJsonFromFile(@"config.json");
        return json;
    }

    //public static string greCaptchaServerKey => ConfigJson["section"]["key"].Value<string>();

}
