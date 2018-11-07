using System;
using System.Collections.Generic;
using HueSharp.Messages.Scenes;
using Newtonsoft.Json;

namespace HueSharp.Converters
{
    class GetAllScenesResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetAllScenesResponse();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var sceneId = reader.Value.ToString();
                    reader.Read();
                    var sceneInfo = serializer.Deserialize<GetSceneResponse>(reader);
                    sceneInfo.Id = sceneId;
                    result.Add(sceneInfo);
                }
            }
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<GetSceneResponse>);
        }
    }
}
