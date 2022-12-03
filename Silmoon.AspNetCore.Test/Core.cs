using Silmoon.AspNetCore.MvcProjectTemplate.DataComponents;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Business.Data.MongoDB;

namespace Silmoon.AspNetCore.Test
{
    public class Core : IDisposable
    {
        MongoConnect MongoConnect { get; set; }
        public MongoExecuter Executer { get; set; }

        public Core()
        {
            MongoConnect = new MongoConnect();
            Executer = new MongoExecuter(MongoConnect.Client, "dbname");

        }
        public User GetUser(string Username)
        {
            return default;
        }


        public void Dispose()
        {
            MongoConnect = null;
            Executer = null;
        }
    }
}
