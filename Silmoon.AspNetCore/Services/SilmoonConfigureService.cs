using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Extension;
using System.IO;

namespace Silmoon.AspNetCore.Services
{
    public class SilmoonConfigureService : ISilmoonConfigureService
    {
        public JObject ConfigJson { get; private set; }
        public SilmoonConfigureServiceOption Options { get; set; }

        public SilmoonConfigureService(IOptions<SilmoonConfigureServiceOption> options)
        {
            Options = options.Value ?? new SilmoonConfigureServiceOption();
            if (File.Exists(Options.ConfigFile))
                ConfigJson = JsonHelperV2.LoadJsonFromFile(Options.ConfigFile);
            else
                ConfigJson = JsonHelperV2.LoadJsonFromFile(Options.DefaultConfigFile);
        }
    }
}
