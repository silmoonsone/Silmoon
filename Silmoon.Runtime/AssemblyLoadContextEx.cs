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
        public AssemblyLoadContextEx()
        {

        }
        public AssemblyLoadContextEx(string? name, bool isCollectible = false) : base(name, isCollectible)
        {

        }
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            Console.WriteLine($"Load => " + assemblyName.FullName);
            return base.Load(assemblyName);
        }
    }
}
