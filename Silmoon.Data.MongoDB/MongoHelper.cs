using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Silmoon.Data.MongoDB.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class FilterHelper
    {
        public static void AddAllJsonConverters(JsonSerializerSettings jsonSerializerSettings)
        {
            jsonSerializerSettings.Converters.Add(new ObjectIdArrayJsonConverter());
            jsonSerializerSettings.Converters.Add(new ObjectIdJsonConverter());
        }
    }
}
