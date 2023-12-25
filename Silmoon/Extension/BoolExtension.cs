using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class BoolExtension
    {
        public static StateSet<bool> ToStateSet(this bool value, string Message = null) => StateSet<bool>.Create(value, Message);
    }
}
