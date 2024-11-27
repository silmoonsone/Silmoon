using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime
{
    public class AssemblyLoadContextEx : AssemblyLoadContext
    {
        public event Func<AssemblyName, Assembly?> OnLoad;
        public AssemblyLoadContextEx(string? name, IEnumerable<string> assemblyNames, IEnumerable<string> assemblyPaths, bool isCollectible = false) : base(name, isCollectible)
        {
            if (assemblyNames is not null)
            {
                foreach (var item in assemblyNames)
                {
                    LoadFromAssemblyName(new AssemblyName(item));
                }
            }
            if (assemblyPaths is not null)
            {
                foreach (var item in assemblyPaths)
                {
                    LoadFromAssemblyPath(new FileInfo(item).FullName);
                }
            }
        }
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var result = OnLoad?.Invoke(assemblyName);
            if (result is null) return base.Load(assemblyName);
            else return result;
        }
    }
}
