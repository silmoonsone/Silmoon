using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Serializer
{
    public class LocalDateTimeSerializer : DateTimeSerializer
    {
        public LocalDateTimeSerializer() : base(DateTimeKind.Local) { }
    }
}
