using System.Reflection;

namespace Silmoon.AspNetCore.Services
{
    public class SilmoonConfigureServiceOption
    {
#if DEBUG
        public string DefaultConfigFile { get; set; } = "config.debug.json";
#else
        public string DefaultConfigFile { get; set; } = "config.json";
#endif
#if DEBUG
        public string ConfigFile { get; set; } = "config-local.debug.json";
#else
        public string ConfigFile { get; set; } = "config-local.json";
#endif
    }
}
