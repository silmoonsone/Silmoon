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
            test();

            bool input = true;
            while (input)
            {
                var str = Console.ReadLine();
                switch (str)
                {
                    case "exit":
                        input = false;
                        break;
                    default:
                        break;
                }
            }


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        static async void test()
        {
            //Action[] actions = new Action[]{
            //    async ()=>{
            //        await Task.Delay(1000);
            //        Console.WriteLine("Action1 finished");
            //    },
            //    async ()=>{
            //        throw new Exception();
            //        await Task.Delay(1000);
            //        Console.WriteLine("Action2 finished");
            //    },
            //    async ()=>{
            //        await Task.Delay(1000);
            //        Console.WriteLine("Action3 finished");
            //    },
            //};


            //Task[] tasks = new Task[]{
            //    new Task(async ()=>{
            //        await Task.Delay(1000);
            //        Console.WriteLine("Action1 finished");
            //    }),
            //    new Task(async ()=>{
            //        await Task.Delay(1000);
            //        //throw new Exception();
            //        Console.WriteLine("Action2 finished");
            //    }),
            //    new Task(async ()=>{
            //        await Task.Delay(1000);
            //        Console.WriteLine("Action3 finished");
            //    }),
            //};



            //var task = Task.Run(async () =>
            //{
            //    await Task.Delay(3000);
            //    Console.WriteLine("task end.");
            //    throw new Exception();
            //});
            var task = new Task(() =>
            {
                Console.WriteLine("task start");
                throw new Exception();
                Console.WriteLine("task end");
            });
            task.Start();

            Console.WriteLine("test end");
        }
    }
}