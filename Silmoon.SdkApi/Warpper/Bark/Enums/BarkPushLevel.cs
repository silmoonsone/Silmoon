using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.SdkApi.Warpper.Bark.Enums
{
    public enum BarkPushLevel
    {
        [JsonProperty("active")]
        Active = 0,
        [JsonProperty("timeSensitive")]
        TimeSensitive = 1,
        [JsonProperty("passive")]
        Passive = 2,
    }
}
