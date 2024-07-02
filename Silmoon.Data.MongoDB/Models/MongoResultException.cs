using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    public class MongoResultException : Exception
    {
        public BsonDocument InnerDocument { get; set; }
        public MongoResultException(BsonDocument innerDocument, string message = null, Exception innerException = null) : base(message, innerException)
        {
            InnerDocument = innerDocument;
        }
    }
}
