using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    public class MongoResultsException : Exception
    {
        public BsonDocument[] InnerDocuments { get; set; }
        public MongoResultsException(BsonDocument[] innerDocuments, string message = null, Exception innerException = null) : base(message, innerException)
        {
            InnerDocuments = innerDocuments;
        }
    }
}
