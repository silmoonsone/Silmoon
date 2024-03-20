using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Extensions
{
    public static class ObjectIdExtension
    {
        public static ObjectId[] GetObjectIds(this string[] ObjectIdStrings)
        {
            if (ObjectIdStrings == null) return null;

            List<ObjectId> ObjectIdList = new List<ObjectId>();
            foreach (var item in ObjectIdStrings)
            {
                ObjectIdList.Add(ObjectId.Parse(item));
            }
            return ObjectIdList.ToArray();
        }
    }
}
