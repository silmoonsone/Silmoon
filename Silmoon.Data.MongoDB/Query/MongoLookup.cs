using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Query
{
    public class MongoLookup
    {
        public string ForeignCollection { get; set; }
        public string LocalField { get; set; }
        public string ForeignField { get; set; }
        public string As { get; set; }

        public static MongoLookup Create(string foreignCollection, string localField, string foreignField, string @as)
        {
            return new MongoLookup() { ForeignCollection = foreignCollection, LocalField = localField, ForeignField = foreignField, As = @as };
        }
    }
}
