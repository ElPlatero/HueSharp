using HueSharp.Messages.Sensors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Converters
{
    class GetAllSensorsResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetAllSensorsResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetAllSensorsResponse();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var sensorId = Convert.ToInt32(reader.Value);
                    reader.Read();
                    var sensor = serializer.Deserialize<SensorBase>(reader);
                    sensor.Id = sensorId;
                    result.Add(sensor);
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
