using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json.Linq;
using Silmoon.Data.MongoDB.Extensions;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Data.MongoDB.Serializer
{
    public class JObjectBsonDocumentConvertSerializer : SerializerBase<JObject>
    {
        public override JObject Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return default;
            }

            var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
            var obj = (BsonDocument)serializer.Deserialize(context);
            return obj.ToJObject();
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JObject value)
        {
            if (value == null) context.Writer.WriteNull();
            else
            {
                var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
                var obj = value.ToBsonDocument();
                serializer.Serialize(context, obj);
            }
        }
    }
}
