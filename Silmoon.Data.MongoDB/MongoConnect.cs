using MongoDB.Driver;
using Silmoon.MySilmoon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class MongoConnect
    {
        public MongoClient Client { get; private set; }
        public MongoUrl MongoUrl { get; private set; }
        string url { get; set; }

        [Obsolete]
        public MongoConnect(MongoClient mongoClient = null)
        {
            if (mongoClient is null) Connect();
            else Client = mongoClient;
        }
        public MongoConnect(string url, bool delayConnect = false)
        {
            this.url = url;
            if (!delayConnect) Connect();
        }
        //void Connect() => Client = new MongoClient(MongoUrl ??= new MongoUrl(Configure.ConfigJson["mongodb"].Value<string>()));
        private void Connect()
        {
            if (MongoUrl is null) MongoUrl = new MongoUrl(url);
            Client = new MongoClient(MongoUrl);
        }
    }
}
