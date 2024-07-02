using MongoDB.Driver;
using Silmoon.MySilmoon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class MongoConnect
    {
        public MongoClient Client { get; set; }
        public MongoUrl MongoUrl { get; set; }
        string url { get; set; }

        [Obsolete]
        public MongoConnect(MongoClient mongoClient = null)
        {
            if (mongoClient is null) Connect();
            else Client = mongoClient;
        }
        public MongoConnect(string Url, bool DelayConnect = false)
        {
            url = Url;
            if (!DelayConnect) Connect();
        }
        //void Connect() => Client = new MongoClient(MongoUrl ??= new MongoUrl(Configure.ConfigJson["mongodb"].Value<string>()));
        void Connect()
        {
            if (MongoUrl is null) MongoUrl = new MongoUrl(url);
            Client = new MongoClient(MongoUrl);
        }
    }
}
