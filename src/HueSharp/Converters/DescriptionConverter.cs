using Newtonsoft.Json;
using System;
using System.Globalization;

namespace HueSharp.Converters
{
    public class DescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullableType = Nullable.GetUnderlyingType(objectType) != null;

            if (reader.TokenType == JsonToken.Null)
            {
                if (!isNullableType)
                {
                    throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert null value to {0}.", objectType.Name));
                }
                return null;
            }

            var underlyingEnumType = isNullableType ? Nullable.GetUnderlyingType(objectType) : objectType;

            try
            {
                if (reader.TokenType == JsonToken.String) return reader.Value.ToString().ToEnum(objectType);
                if (reader.TokenType == JsonToken.Integer) return Convert.ChangeType(reader.Value, objectType);
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Error converting value {0} to type '{1}'.", reader.Value, objectType), ex);
            }

            throw new JsonSerializationException(string.Format(CultureInfo.InvariantCulture, "Unexpected token {0} when parsing enum.", reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) writer.WriteNull();
            else
            {
                Enum e = (Enum)value;
                var description = e.ToDescription();
                writer.WriteValue(char.IsNumber(description[0]) || description.StartsWith("-") ? value : description);
            }
        }
    }
}
