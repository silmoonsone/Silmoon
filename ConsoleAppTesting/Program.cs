using ConsoleAppTesting;
using Silmoon.Extension;
using System.Linq.Expressions;



Expression<Func<User, User>> expression = x => new User()
{
    Flag = 2,
    Password = "123123"
};

var result = expression.GetMemberAssignment();

foreach (var item in result)
{
    Console.WriteLine($"{item.Name}({item.Type.Name}) = {item.Value}");
}


Console.ReadKey();
