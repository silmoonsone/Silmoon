using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Silmoon.AspNetCore.Test;
using Silmoon.Business.Data.MongoDB;

namespace Silmoon.AspNetCore.MvcProjectTemplate.DataComponents
{
    public class MongoConnect : IMongoConnecter
    {
        private string MongoDBUrl { get; set; }
        public MongoClient Client { get; set; }
        public IMongoDatabase DefaultDatabase => Client.GetDatabase("dbname");

        public MongoConnect(MongoClient mongoClient = null)
        {
            if (mongoClient is null) Connect();
            else Client = mongoClient;
        }
        void Connect() => Client = new MongoClient(MongoDBUrl ??= Configure.ConfigJson["mongodb"].Value<string>());
    }
}
