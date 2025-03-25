using ConsoleAppTesting;
using Silmoon.Extension;
using System.Linq.Expressions;



//Expression<Func<User, User>> expression = x => new User()
//{
//    Flag = 2,
//    Password = "123123"
//};

//var result = expression.GetMemberAssignment();

//foreach (var item in result)
//{
//    Console.WriteLine($"{item.Name}({item.Type.Name}) = {item.Value}");
//}

User user = new User()
{
    Username = "admin",
    Password = "123456"
};

Expression<Func<App, App>> expression2 = x => new App()
{
    AppId = user.Username,
    created_at = DateTime.Now,
};

var result2 = expression2.GetMemberAssignment();



Console.ReadKey();
