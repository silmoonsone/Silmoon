using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json.Linq;
using Silmoon.Data.MongoDB.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Data.MongoDB.Serializer
{
    public class JArrayBsonDocumentConvertSerializer : SerializerBase<JArray>
    {
        public override JArray Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var serializer = BsonSerializer.LookupSerializer(typeof(BsonArray));
            var obj = (BsonArray)serializer.Deserialize(context);
            return obj.ToJArray();
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JArray value)
        {
            if (value == null) context.Writer.WriteNull();
            else
            {
                var serializer = BsonSerializer.LookupSerializer(typeof(BsonArray));
                var obj = value.ToBsonArray();
                serializer.Serialize(context, obj);
            }
        }
    }
}
