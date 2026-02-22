using ConsoleAppTesting;
using Silmoon;
using Silmoon.Extension;
using Silmoon.Threading;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Testing;


internal class Program
{
    static AsyncLock asyncLock = AsyncLock.Create();
    private static async Task Main(string[] args)
    {
        ExpressionExtensionTest();
        //ExtensionTest.StringExtensionTest();
        //ExtensionTest.NumberExtensionTest();
        //await AsyncLockTest();
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

    static async Task AsyncLockTest()
    {
        _ = AsyncWorkLockTest();
        _ = AsyncWorkLockTest();
        await AsyncWorkLockTest();
    }
    static async Task AsyncWorkLockTest()
    {
        Console.WriteLine($"Task {Environment.CurrentManagedThreadId} is waiting to acquire the lock.");
        using (await asyncLock.LockAsync())
        {
            Console.WriteLine($"Task {Environment.CurrentManagedThreadId} acquired the lock.");
            Console.WriteLine($"Task {Environment.CurrentManagedThreadId} is doing some work...");
            await Task.Delay(1000); // Simulate some asynchronous work
            Console.WriteLine($"Task {Environment.CurrentManagedThreadId} releasing the lock.");
        }
    }
}