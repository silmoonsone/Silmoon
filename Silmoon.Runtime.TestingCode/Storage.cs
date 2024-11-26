global using System;
global using System.Collections.Generic;

namespace Silmoon.Runtime.TestingCode;

public class Storage : IStorage
{
    private Dictionary<string, byte[]> _storage { get; } = [];

    public event Action<string, byte[]> OnSet;
    public byte[] Get(string key)
    {
        _storage.TryGetValue(key, out byte[] result);
        Test();
        return result;
    }

    public void Set(string key, byte[] value)
    {
        _storage[key] = value;
        OnSet?.Invoke(key, value);
    }

    public void Test()
    {
        Console.WriteLine("Test");
        Environment.Exit(0);
    }
}