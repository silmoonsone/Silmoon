using Newtonsoft.Json;
using Silmoon.Data.MongoDB.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Extensions
{
    public static class JsonSerializerSettingsBsonExtension
    {
        public static void AddAllBsonConverters(this JsonSerializerSettings jsonSerializerSettings)
        {
            jsonSerializerSettings.Converters.Add(new ObjectIdArrayJsonConverter());
            jsonSerializerSettings.Converters.Add(new ObjectIdJsonConverter());
        }
    }
}
