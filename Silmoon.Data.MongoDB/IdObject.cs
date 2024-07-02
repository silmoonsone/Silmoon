using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Silmoon.Data.MongoDB.Converters;
using Silmoon.Data.MongoDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    [Serializable]
    public class IdObject : IIdObject, ICreatedAt
    {
        [Newtonsoft.Json.JsonConverter(typeof(ObjectIdJsonConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(ObjectIdStringJsonConverter))]
        public ObjectId _id { get; set; } = ObjectId.GenerateNewId();
        [DisplayName("创建日期")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime created_at { get; set; } = DateTime.Now;
    }
}
