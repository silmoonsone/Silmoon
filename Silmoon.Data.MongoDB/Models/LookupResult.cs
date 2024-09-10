using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    public class LookupResult<T>
    {
        public T Result { get; set; }
        public Dictionary<string, BsonDocument> AttachDocuments { get; set; } = new Dictionary<string, BsonDocument>();
    }
}
