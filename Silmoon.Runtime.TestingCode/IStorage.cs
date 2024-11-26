namespace Silmoon.Runtime.TestingCode;

public interface IStorage
{
    event Action<string, byte[]> OnSet;
    byte[] Get(string key);
    void Set(string key, byte[] value);
}