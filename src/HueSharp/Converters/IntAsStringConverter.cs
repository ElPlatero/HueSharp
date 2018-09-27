using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HueSharp.Converters
{
    public class IntAsStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<int>) || objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            if (reader.TokenType == JsonToken.StartArray)
            {
                var result = new List<int>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    result.Add(Convert.ToInt32(reader.Value));
                }
                return result;
            }
            else return Convert.ToInt32(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is int) writer.WriteValue(value.ToString());
            else
            {
                writer.WriteStartArray();
                ((IEnumerable<int>)value).Select(p => p.ToString()).ToList().ForEach(writer.WriteValue);
                writer.WriteEndArray();
            }
        }
    }
}
