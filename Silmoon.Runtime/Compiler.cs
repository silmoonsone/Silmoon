﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime
{
    public class Compiler
    {
        public static CSharpCompilationOptions GetDefaultCSharpCompilerOptions()
        {
            return new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    checkOverflow: true,
                    optimizationLevel: OptimizationLevel.Release,
                    deterministic: true);
        }
        public async Task<CompilerResult> CompileSourceFilesAsync(string assemblyName, IEnumerable<string> filePaths, CSharpCompilationOptions compilationOptions = null, string[] referrerAssemblyPaths = null, string[] referrerAssemblyNames = null, bool isReferrerCurrentAssembly = false)
        {
            var syntaxTrees = new List<SyntaxTree>();

            foreach (var filePath in filePaths)
            {
                string sourceCode = await File.ReadAllTextAsync(filePath);
                var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
                syntaxTrees.Add(syntaxTree);
            }

            return CompileAsync(syntaxTrees, assemblyName, compilationOptions, referrerAssemblyPaths, referrerAssemblyNames, isReferrerCurrentAssembly);
        }
        public CompilerResult CompileAsync(IEnumerable<SyntaxTree> syntaxTrees, string assemblyName, CSharpCompilationOptions compilationOptions = null, string[] referrerAssemblyPaths = null, string[] referrerAssemblyNames = null, bool isReferrerCurrentAssembly = false)
        {
            if (compilationOptions is null) compilationOptions = GetDefaultCSharpCompilerOptions();
            List<MetadataReference> references = [];

            if (!referrerAssemblyPaths.IsNullOrEmpty()) referrerAssemblyPaths.Each(fileName => references.Add(MetadataReference.CreateFromFile(fileName)));

            if (!referrerAssemblyNames.IsNullOrEmpty()) referrerAssemblyNames.Each(assemblyName => references.Add(MetadataReference.CreateFromFile(Assembly.Load(assemblyName).Location)));

            if (isReferrerCurrentAssembly)
            {
                var additionalReferences = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !a.Location.IsNullOrEmpty()).Select(a => MetadataReference.CreateFromFile(a.Location));
                references.AddRange(additionalReferences);
            }
            //compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, deterministic: true);

            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, references, compilationOptions);

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            return new CompilerResult
            {
                Success = result.Success,
                Binary = result.Success ? ms.ToArray() : null,
                Diagnostics = result.Diagnostics,
            };
        }
    }
}
