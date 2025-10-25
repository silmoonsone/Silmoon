using Microsoft.CodeAnalysis;
using Mono.Cecil;
using Silmoon.Runtime;
using Silmoon.Extension;
using Silmoon.Secure;
using Silmoon.Runtime.Extensions;
using Mono.Cecil.Rocks;
using Silmoon.Runtime.Testing;


//byte[] assemblyBytes = File.ReadAllBytes("../../../../SourceCode.Storage/bin/Debug/net8.0/SourceCode.Storage.dll");
string[] sourceCodeFiles = [
        @"../../../../Silmoon.Runtime.TestingCode/MyStorage.cs",
        @"../../../../Silmoon.Runtime.TestingCode/Storage.cs",
        @"../../../../Silmoon.Runtime.TestingCode/IStorage.cs",
    ];

Compiler compiler = new Compiler();
//var result = await compiler.CompileSourceFilesAsync("StorageAssembly", sourceCodeFiles, null, [
//        @"C:/Program Files/dotnet/shared/Microsoft.NETCore.App/8.0.8/System.Console.dll",
//        @"C:/Program Files/dotnet/shared/Microsoft.NETCore.App/8.0.8/System.Runtime.dll",
//        @"C:/Program Files/dotnet/shared/Microsoft.NETCore.App/8.0.8/System.Collections.dll",
//        @"C:/Program Files/dotnet/shared/Microsoft.NETCore.App/8.0.8/System.Private.CoreLib.dll",
//    ], null, false);

var result = await compiler.CompileSourceFilesAsync("StorageAssembly", sourceCodeFiles, null, null, ["System.Console", "System.Runtime", "System.Collections", "System.Private.CoreLib"], false);

if (!result.Success)
{
    result.Diagnostics.Each(diagnostic => Console.WriteLine(diagnostic));
}
else
{
    Console.WriteLine($"Compilation success...(size: {result.Binary.Length}, hash:{result.Binary.GetSHA256Hash().ToHexString()})");
    Console.WriteLine();
    var assemblyDefinition = AssemblyDefinition.ReadAssembly(new MemoryStream(result.Binary));
    var typeDefinition = assemblyDefinition.MainModule.GetType("Silmoon.Runtime.TestingCode.MyStorage");

    if (typeDefinition is null)
    {
        Console.WriteLine($"Type {typeDefinition.FullName} not found in assembly");
    }
    else
    {
        //Console.WriteLine("Base Type: " + typeDefinition.BaseType.FullName);
        //typeDefinition.Interfaces.ForEachEx(i => Console.WriteLine("Base Interfaces:" + i.InterfaceType.FullName));

        var interfaces = typeDefinition.GetAllInterfaces();
        interfaces.Each(i => Console.WriteLine("Interfaces:" + i.InterfaceType.FullName));
        Console.WriteLine();
        var baseTypes = typeDefinition.GetAllBaseTypes();
        baseTypes.Each(b => Console.WriteLine("Base Types:" + b.FullName));

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=".Repeat(Console.BufferWidth));
        Console.ResetColor();
        Console.WriteLine("Running...");
        Console.WriteLine();

        while (true)
        {
            var context = new AssemblyLoadContextEx("MyAssembly", null, null, true);
            using var codeStream = result.Binary.GetStream();
            var assembly = context.LoadFromStream(codeStream);
            Console.WriteLine("Load assembly binarycode ok");
            Console.WriteLine();

            var type = assembly.GetType(typeDefinition.FullName);
            var instance = Activator.CreateInstance(type);


            var eventInfo = type.GetEvent("OnSet");
            eventInfo.AddEventHandler(instance, new Action<string, byte[]>((name, data) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Event => SetKeyName: {name}, DataLength: {data.Length}");
                Console.ResetColor();
            }));

            Console.WriteLine("Execute SetName()");
            type.GetMethod("SetName")?.Invoke(instance, ["Hello World"]);

            var test = typeDefinition.GetMethods();
            var test2 = typeDefinition.Methods[1].Body.Instructions[0];

            var usedTypes = TestHelper.GetUsedTypes(typeDefinition);

            Console.WriteLine("GetName() result: " + type.GetMethod("GetName")?.Invoke(instance, null));



            Console.WriteLine();

            type.GetMethod("Set")?.Invoke(instance, ["data", new byte[1024000000]]);
            context.Unload();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.ReadLine();
        }
    }
}