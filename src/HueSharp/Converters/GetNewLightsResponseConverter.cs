using HueSharp.Messages.Lights;
using Newtonsoft.Json;
using System;

namespace HueSharp.Converters
{
    class GetNewLightsResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetNewLightsResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetNewLightsResponse();
            while(reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName && reader.Value.ToString().Equals("lastscan"))
                {
                    reader.Read();
                    if (reader.Value.ToString().Equals("none")) result.LastScan = DateTime.MinValue;
                    else if (reader.Value.ToString().Equals("active")) result.LastScan = DateTime.MaxValue;
                    else result.LastScan = DateTime.Parse(reader.Value.ToString());

                }
                else if(reader.TokenType == JsonToken.PropertyName)
                {
                    var newBasicLight = new BasicLight {Id = Convert.ToInt32(reader.Value)};
                    reader.Read();
                    newBasicLight.Name = reader.Value.ToString();
                    result.Add(newBasicLight);
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
