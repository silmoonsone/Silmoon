using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB
{
    public class MongoConnect
    {
        public MongoClient Client { get; private set; }
        public MongoUrl MongoUrl { get; private set; }
        string ConnectionString { get; set; }

        public MongoConnect(string connectionString, bool delayConnect = false)
        {
            ConnectionString = connectionString;
            if (!delayConnect) Connect();
        }
        //void Connect() => Client = new MongoClient(MongoUrl ??= new MongoUrl(Configure.ConfigJson["mongodb"].Value<string>()));
        private void Connect()
        {
            if (MongoUrl is null) MongoUrl = new MongoUrl(ConnectionString);
            Client = new MongoClient(MongoUrl);
        }
    }
}
