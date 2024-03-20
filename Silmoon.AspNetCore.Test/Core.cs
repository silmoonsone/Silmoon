using Newtonsoft.Json.Linq;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Data.MongoDB.MongoDB;

namespace Silmoon.AspNetCore.Test
{
    public class Core : IDisposable
    {
        public MongoExecuter Executer { get; set; }

        public Core()
        {
            Executer = new MongoExecuter(new MongoConnect(Configure.ConfigJson["mongodb"].Value<string>()));
        }
        public User GetUser(string Username)
        {
            return new User()
            {
                Username = "silmoon",
                Password = "pwd",
            };
        }


        public void Dispose()
        {
            Executer = null;
        }
    }
}
