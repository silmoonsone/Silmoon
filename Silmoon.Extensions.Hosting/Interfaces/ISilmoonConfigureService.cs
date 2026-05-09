using Newtonsoft.Json.Linq;
using Silmoon.Extensions.Hosting.Services;

namespace Silmoon.Extensions.Hosting.Interfaces;

public interface ISilmoonConfigureService
{
    public JObject ConfigJson { get; }
    string CurrentConfigFile { get; }
    //SilmoonConfigureServiceOption Options { get; }
}
