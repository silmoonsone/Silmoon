using MongoDB.Bson;
using Silmoon.Data.MongoDB.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    public interface IIdObject
    {
        [Newtonsoft.Json.JsonConverter(typeof(ObjectIdJsonConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(ObjectIdStringJsonConverter))]
        ObjectId _id { get; set; }
    }
}
