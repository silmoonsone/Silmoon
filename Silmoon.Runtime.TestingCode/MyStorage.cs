using System.Text;

namespace Silmoon.Runtime.TestingCode;

public class MyStorage : Storage, IStorage
{
    public MyStorage()
    {

    }
    public void SetName(string name)
    {
        Set("Name", Encoding.UTF8.GetBytes(name));
    }
    public string GetName()
    {
        return Encoding.UTF8.GetString(Get("Name"));
    }
    ~MyStorage()
    {
        Console.WriteLine("Finalized");
    }
}