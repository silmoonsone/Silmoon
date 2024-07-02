using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Silmoon.Data.MongoDB.Serializer
{
    public class BigIntegerConvertSerializer : SerializerBase<BigInteger>
    {
        public override BigInteger Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.String)
            {
                var val = context.Reader.ReadString();
                return BigInteger.Parse(val.ToString());
            }
            else if (context.Reader.CurrentBsonType == BsonType.Int32 || context.Reader.CurrentBsonType == BsonType.Int64)
            {
                var val = context.Reader.ReadInt64();
                return (BigInteger)val;
            }
            else return 0;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BigInteger value)
        {
            context.Writer.WriteString(value.ToString());
        }
    }
}
