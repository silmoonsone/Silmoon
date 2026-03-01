using Newtonsoft.Json.Linq;

namespace Silmoon.Extensions.Hosting.Interfaces;
public interface ISilmoonConfigureService
{
    public JObject ConfigJson { get; }
}
