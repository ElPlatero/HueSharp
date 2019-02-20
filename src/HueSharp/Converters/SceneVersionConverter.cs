using System;
using Newtonsoft.Json;

namespace HueSharp.Converters
{
    class SceneVersionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) 
            => writer.WriteValue((bool)value ? 2 : 1);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) 
            => int.TryParse(reader.Value.ToString(), out var result) && result == 2;

        public override bool CanConvert(Type objectType) => objectType == typeof(bool);
    }
}
