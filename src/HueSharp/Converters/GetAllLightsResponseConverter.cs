using HueSharp.Messages.Lights;
using Newtonsoft.Json;
using System;

namespace HueSharp.Converters
{
    class GetAllLightsResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetAllLightsResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetAllLightsResponse();

            while(reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName)
                {
                    var lightId = Convert.ToInt32(reader.Value);
                    reader.Read();
                    var subSerializer = new JsonSerializer();
                    var light = subSerializer.Deserialize<Light>(reader);
                    light.Id = lightId;
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
