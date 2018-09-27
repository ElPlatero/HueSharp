using HueSharp.Messages.Sensors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HueSharp.Converters
{
    class SensorBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SensorBase).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return SensorBase.Create(JObject.Load(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
