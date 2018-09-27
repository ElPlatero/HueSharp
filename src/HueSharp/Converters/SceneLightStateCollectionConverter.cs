using HueSharp.Messages.Scenes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HueSharp.Converters
{
    class SceneLightStateCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<SceneLightState>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<SceneLightState>();

            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var lightId = Convert.ToInt32(reader.Value);
                    reader.Read();
                    var light = serializer.Deserialize<SceneLightState>(reader);
                    light.LightId = lightId;
                    result.Add(light);
                }
            }
            return result;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
