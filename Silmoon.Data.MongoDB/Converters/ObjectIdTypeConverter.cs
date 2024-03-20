using MongoDB.Bson;
using System.ComponentModel;
using System.Globalization;
using System;

namespace Silmoon.Data.MongoDB.Converters
{
    /// <summary>
    /// 用于ObjectId类型进行序列化和反序列化的转换器，更加底层。
    /// </summary>
    public class ObjectIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var strValue = value as string;
            return strValue != null ? new ObjectId(strValue) : base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string)) return value.ToString();
            else return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
