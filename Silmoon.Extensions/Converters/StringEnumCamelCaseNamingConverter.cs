using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Converters
{
    public class StringEnumCamelCaseNamingConverter : StringEnumConverter
    {
        public StringEnumCamelCaseNamingConverter() : base(new CamelCaseNamingStrategy())
        {
        }
    }
}
