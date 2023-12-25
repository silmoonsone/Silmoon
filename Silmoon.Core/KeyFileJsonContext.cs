using Silmoon.Core.Models;
using System.Text.Json.Serialization;

namespace Silmoon.Core
{
    [JsonSerializable(typeof(KeyFile))]
    public partial class KeyFileJsonContext : JsonSerializerContext
    {
    }
}
