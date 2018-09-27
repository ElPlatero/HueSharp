using System;
using Newtonsoft.Json;

namespace HueSharp.Converters
{
    class UtcDateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToUniversalTime());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ((DateTime)reader.Value).ToLocalTime();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}
