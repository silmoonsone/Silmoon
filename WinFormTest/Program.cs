using Silmoon;

namespace WinFormTest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UrlDataCollection urlDataCollection = new UrlDataCollection();
            urlDataCollection.Add("from_name", "1");
            urlDataCollection.Add("from", "1");
            var str = urlDataCollection.ToQueryString(true, true);
            //bool input = true;
            //while (input)
            //{
            //    var str = Console.ReadLine();
            //    switch (str)
            //    {
            //        case "exit":
            //            input = false;
            //            break;
            //        default:
            //            break;
            //    }
            //}


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}