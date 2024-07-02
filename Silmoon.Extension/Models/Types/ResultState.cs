using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension.Models.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultState
    {
        Exception = -3,
        FormatError = -2,
        NotHttpSuccess = -1,
        Unknown = 0,
        Success = 1,
        Info = 2,
        Fail = 3,
    }
}
