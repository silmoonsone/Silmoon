using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Silmoon.AspNetCore.Extension.CoreHelpers;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Data.MongoDB;
using Silmoon.Data.MongoDB.MongoDB;
using Silmoon.Extension;
using Silmoon.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Silmoon.AspNetCore.Test;

public class Core : MongoService, IDisposable
{
    public override MongoExecuter Executer { get; set; }

    public Core()
    {
        Executer = new MongoExecuter(new MongoConnect(Configure.ConfigJson["mongodb"].Value<string>()));

    }
    public User GetUser(string Username)
    {
        if (Username.IsNullOrEmpty()) return null;
        return new User()
        {
            Username = Username,
            Password = "123",
        };
    }
    public StateSet<bool> NewUser(User user)
    {
        return true.ToStateSet(user.Username);
    }

    public void Dispose()
    {
        Executer = null;
    }

}
