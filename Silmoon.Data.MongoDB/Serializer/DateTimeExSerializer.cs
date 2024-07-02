using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Serializer
{
    public class DateTimeExSerializer : DateTimeSerializer
    {
        public DateTimeKind DateTimeKind { get; set; } = DateTimeKind.Unspecified;
        public DateTimeExSerializer(DateTimeKind dateTimeKind = DateTimeKind.Local)
        {
            DateTimeKind = dateTimeKind;
        }
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var localDateTime = DateTime.SpecifyKind(value, DateTimeKind);
            base.Serialize(context, args, localDateTime);
        }

        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var dateTime = base.Deserialize(context, args);
            return DateTime.SpecifyKind(dateTime, DateTimeKind);
        }
    }
}
