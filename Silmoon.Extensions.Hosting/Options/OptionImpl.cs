using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Hosting.Options
{
    public class OptionImpl<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        public OptionImpl(TOptions silmoonConfigureServiceOption = null) => Value = silmoonConfigureServiceOption ?? new TOptions();
        public TOptions Value { get; set; }
    }
}
