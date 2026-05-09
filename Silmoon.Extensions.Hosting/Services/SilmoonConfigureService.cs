using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon.Extensions.Hosting.Interfaces;
using Silmoon.Extensions.Hosting.Options;
using System;
using System.IO;

namespace Silmoon.Extensions.Hosting.Services;

public class SilmoonConfigureService : ISilmoonConfigureService
{
    public JObject ConfigJson { get; private set; }
    private SilmoonConfigureServiceOption Options { get; set; }
    public string CurrentConfigFile { get; private set; }

    public SilmoonConfigureService(IOptions<SilmoonConfigureServiceOption> options, ISilmoonPlatformDirectoryService silmoonPlatformDirectoryService = null)
    {
        Options = options.Value;
        if (Options.IsDebug is null)
        {
            //抛出异常，必须指定是否是Debug模式，使用SilmoonConfigureServiceOption.DebugConfig()或SilmoonConfigureServiceOption.ReleaseConfig()方法指定。
            throw new Exception("IsDebug must be specified, use SilmoonConfigureServiceOption.DebugConfig() or SilmoonConfigureServiceOption.ReleaseConfig() method to specify.");
        }
        else
        {
            if (silmoonPlatformDirectoryService is null)
            {
                if (File.Exists(Options.LocalConfigFile))
                    CurrentConfigFile = Options.LocalConfigFile;
                else
                    CurrentConfigFile = Options.DefaultConfigFile;
            }
            else
            {
                if (File.Exists(Path.Combine(silmoonPlatformDirectoryService.AppConfigDirectory, Options.LocalConfigFile)))
                    CurrentConfigFile = Options.LocalConfigFile;
                else
                    CurrentConfigFile = Options.DefaultConfigFile;
            }


            if (silmoonPlatformDirectoryService is null)
                ConfigJson = JsonHelperV2.LoadJsonFromFile(CurrentConfigFile);
            else
                ConfigJson = JsonHelperV2.LoadJsonFromFile(Path.Combine(silmoonPlatformDirectoryService.AppConfigDirectory, CurrentConfigFile));
        }
    }
    public static SilmoonConfigureService CreateSingleton(SilmoonConfigureServiceOption options = null) => new SilmoonConfigureService(new OptionImpl<SilmoonConfigureServiceOption>(options), null);
}
