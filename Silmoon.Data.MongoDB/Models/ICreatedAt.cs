using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data.MongoDB.Models
{
    public interface ICreatedAt
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        DateTime created_at { get; set; }
    }
}
