using Newtonsoft.Json.Linq;
using Silmoon;
using Silmoon.Extension;
using Silmoon.Extension.Http;

namespace WinFormTest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Args.ParseArgs(args);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());

            Test().Wait();
        }
        static async Task Test()
        {
            JObject json = new JObject();
            json["name"] = "silmoon";
            var result = await JsonRequest.PostAsync<JObject, JObject>("https://localhost:7005/api/TestApi/PostJsonTest", json, null);
            Console.WriteLine(result.ToFormattedJsonString());
        }
    }
}