using HueSharp.Messages;
using Newtonsoft.Json;
using System;

namespace HueSharp.Converters
{
    class ErrorResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ErrorResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new ErrorResponse();

            while(reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName && reader.Value.ToString().Equals("error") && reader.Read())
                {
                    result.Add(serializer.Deserialize<ErrorMessage>(reader));
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
