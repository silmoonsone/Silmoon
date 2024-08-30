using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Converters
{
    public class StringEnumCamelCaseNamingConverter : StringEnumConverter
    {
        public StringEnumCamelCaseNamingConverter() : base(new CamelCaseNamingStrategy())
        {
        }
    }
}
