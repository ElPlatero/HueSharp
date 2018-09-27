using System;
using System.ComponentModel;
using System.Linq;

namespace HueSharp
{
    public static class Extensions
    {
        public static string ToDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static Enum ToEnum(this string value, Type enumType)
        {
            var underlyingType = Nullable.GetUnderlyingType(enumType);

            if (string.IsNullOrEmpty(value) && underlyingType != null) return null;

            if (underlyingType != null) enumType = underlyingType;

            var map = Enum.GetValues(enumType).OfType<Enum>().ToDictionary(p => p.ToDescription());

            if (value.IndexOf(',') != -1)
            {
                var names = value.Split(',').Select(p => p.Trim()).Select(p => map.ContainsKey(p) ? map[p] : null).Where(p => p != null);
                return (Enum)Enum.Parse(enumType, string.Join(", ", names));
            }

            if (map.ContainsKey(value)) return map[value];

            return (Enum)Enum.Parse(enumType, value, true);
        }
    }
}
