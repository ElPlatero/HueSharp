using HueSharp.Messages.Sensors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Converters
{
    class GetNewSensorsResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetNewSensorsResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetNewSensorsResponse();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString().Equals("lastscan"))
                {
                    reader.Read();
                    if (reader.Value.ToString().Equals("none")) result.LastScan = DateTime.MinValue;
                    else if (reader.Value.ToString().Equals("active")) result.LastScan = DateTime.MaxValue;
                    else result.LastScan = DateTime.Parse(reader.Value.ToString());

                }
                else if (reader.TokenType == JsonToken.PropertyName)
                {
                    var newBasicSensor = new BasicSensor();
                    newBasicSensor.Id = Convert.ToInt32(reader.Value);
                    reader.Read();
                    newBasicSensor.Name = reader.Value.ToString();
                    result.Add(newBasicSensor);
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
