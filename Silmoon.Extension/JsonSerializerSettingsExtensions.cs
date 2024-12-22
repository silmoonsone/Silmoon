using Newtonsoft.Json;
using Silmoon.Extension.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class JsonSerializerSettingsExtensions
    {
        public static void AddAllCommonConverters(this JsonSerializerSettings jsonSerializerSettings)
        {
            jsonSerializerSettings.Converters.Add(new IPAddressJsonConverter());
            jsonSerializerSettings.Converters.Add(new HostEndPointJsonConverter());
            jsonSerializerSettings.Converters.Add(new BigIntegerJsonConverter());
        }
    }
}
