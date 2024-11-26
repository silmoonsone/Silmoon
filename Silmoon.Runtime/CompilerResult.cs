using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime
{
    public class CompilerResult
    {
        public bool Success { get; set; }
        public byte[] Binary { get; set; }
        public ImmutableArray<Diagnostic> Diagnostics { get; set; }
    }
}
