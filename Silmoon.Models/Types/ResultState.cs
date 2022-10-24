using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Models.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultState
    {
        DataFormatError = -2,
        Unknown = 0,
        Success = 1,
        Info = 2,
        Fail = 3,
        Exception = 5,
    }
}
