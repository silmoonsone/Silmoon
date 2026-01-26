using ConsoleAppTesting;
using Silmoon;
using Silmoon.Extension;
using Silmoon.Threading;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


internal class Program
{
    static AsyncLock asyncLock = AsyncLock.Create();
    private static async Task Main(string[] args)
    {
        await AsyncLockTest();
        //NumberExtensionTest();
        //StringExtensionTest();
        //ExpressionExtensionTest();
    }
    static void ExpressionExtensionTest()
    {
        Expression<Func<User, User>> expression = x => new User() { Flag = 2, Password = "123123" };
        var result = expression.GetMemberAssignment();
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Name}({item.Type.Name}) = {item.Value}");
        }
        Console.WriteLine();


        User user = new User() { Username = "admin", Password = "123456" };
        Expression<Func<App, App>> expression2 = x => new App()
        {
            AppId = user.Username,
            created_at = DateTime.Now,
        };
        var result2 = expression2.GetMemberAssignment();
        foreach (var item in result2)
        {
            Console.WriteLine($"{item.Name}({item.Type.Name}) = {item.Value}");
        }
        Console.WriteLine();

        var result3 = ExpressionExtension.GetPreprotyNamesExpressions<User>([x => x.Username, x => x.Password]);
        Console.WriteLine(result3.ToJsonString());
        Console.WriteLine();
    }
    static void StringExtensionTest()
    {
        string chinese = "你好世界";
        string englishAndChinese = "Hello世界";
        Console.WriteLine(chinese.GetDisplayWidth());
        Console.WriteLine(chinese.GetEncodingByteCount(Encoding.UTF8));
        Console.WriteLine(englishAndChinese.SubstringDisplayWidth(4, 3));
        Console.WriteLine();


        byte[] bytes = [0, 1, 2, 3, 4, 5];
        var hexString = bytes.ToHexString(true, true);
        Console.WriteLine(hexString);
        var result = hexString.HexStringToByteArray();
        Console.WriteLine(string.Join(", ", result.Data));
        Console.WriteLine();
    }
    static void NumberExtensionTest()
    {
        decimal a = -1.03m;
        double b = 2;
        float c = 3;
        decimal d = 4;

        Console.WriteLine(a.Negative());
        Console.WriteLine(b.Negative());
        Console.WriteLine(c.Negative());
        Console.WriteLine(d.Negative());
    }

    static async Task AsyncLockTest()
    {
        _ = AsyncWorkLockTest();
        _ = AsyncWorkLockTest();
        await AsyncWorkLockTest();
    }
    static async Task AsyncWorkLockTest()
    {
        Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} is waiting to acquire the lock.");
        using (await asyncLock.LockAsync())
        {
            Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} acquired the lock.");
            Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} is doing some work...");
            await Task.Delay(1000); // Simulate some asynchronous work
            Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} releasing the lock.");
        }
    }
}