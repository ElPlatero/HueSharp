using HueSharp.Messages.Groups;
using Newtonsoft.Json;
using System;

namespace HueSharp.Converters
{
    class GetAllGroupsResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetAllGroupsResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetAllGroupsResponse();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var groupId = Convert.ToInt32(reader.Value);
                    reader.Read();
                    var subSerializer = new JsonSerializer();
                    var group = subSerializer.Deserialize<GetGroupResponse>(reader);
                    group.Id = groupId;
                    result.Add(group);
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
