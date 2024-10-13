using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silmoon;
using Silmoon.Data.MongoDB.Converters;
using Silmoon.Data.MongoDB.Serializer;
using Silmoon.Extension;
using Silmoon.Extension.Http;
using System.Collections.ObjectModel;

namespace WinFormTest;
internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main(string[] args)
    {
        //Args.ParseArgs(args);

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        //if (ForeachExTest()) return;
        //if (JsonTest()) return;
        //if (await Test()) return;

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
        await Task.CompletedTask;
    }
    static async Task<bool> Test()
    {
        JObject json = new JObject();
        json["name"] = "silmoon";
        var result = await JsonRequest.PostAsync<JObject, JObject>("https://localhost:7005/api/TestApi/PostJsonTest", json, null);
        Console.WriteLine(result.ToFormattedJsonString());
        return true;
    }
    static bool JsonTest()
    {
        JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdJsonConverter());
            return settings;
        });
        BsonSerializer.RegisterSerializer(typeof(JObject), new JObjectBsonDocumentConvertSerializer());


        string str = "66716225527bf03331c3b18b";
        //str = null;

        ObjectId? objId = ObjectId.Parse(str);
        objId = null;


        var obj = new { name = "silmoon", _id = objId };
        JObject jobj = JObject.FromObject(obj);
        var objectId = jobj["_id"].ToObject<ObjectId?>();

        return true;
    }
    static bool ForeachExTest()
    {
        int[] intArray = [1, 2, 3, 4, 5];
        intArray.Each(Console.WriteLine);
        List<int> listArray = [1, 2, 3, 4, 5];
        listArray.Each(Console.WriteLine);

        ObservableCollection<int> collection = [1, 2, 3, 4, 5];
        collection.Each(Console.WriteLine);

        return true;
    }
}