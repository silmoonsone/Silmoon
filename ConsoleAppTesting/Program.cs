using ConsoleAppTesting;
using Silmoon.Extension;
using System.Linq.Expressions;
using System.Text;


internal class Program
{
    private static void Main(string[] args)
    {
        StringExtensionTest();
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
}