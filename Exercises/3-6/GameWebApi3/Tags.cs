using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class CustomEnumConverter<T> : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(@"""" + value.ToString() + @"""");
        }
    }

    [TypeConverter(typeof(CustomEnumConverter<Tags>))]
    public enum Tags
    {
        [EnumMember(Value = "active")]
        active,
        [EnumMember(Value = "inactive")]
        inactive
    }

}
