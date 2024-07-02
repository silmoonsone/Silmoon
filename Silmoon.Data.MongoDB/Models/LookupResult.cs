using MongoDB.Bson;
using Silmoon.Runtime.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    public class LookupResult<T>
    {
        public T Result { get; set; }
        public DictionaryEx<string, BsonDocument> AttachDocuments { get; set; } = new DictionaryEx<string, BsonDocument>();
    }
}
