using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class FilterHelper
    {
        public static FilterDefinition<T> GetRegexFilterDefinition<T>(FilterDefinitionBuilder<T> filterDefinitionBuilder, FieldDefinition<T> fieldDefinition, string regex)
        {
            return filterDefinitionBuilder.Regex(fieldDefinition, new BsonRegularExpression(regex));
        }
    }
}
