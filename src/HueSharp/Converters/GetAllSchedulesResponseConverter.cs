using System;
using HueSharp.Messages.Schedules;

using Newtonsoft.Json;

namespace HueSharp.Converters
{
    class GetAllSchedulesResponseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new GetAllSchedulesResponse();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var scheduleId = Convert.ToInt32(reader.Value);
                    reader.Read();
                    var subSerializer = new JsonSerializer();
                    var schedule = subSerializer.Deserialize<GetScheduleResponse>(reader);
                    schedule.Id = scheduleId;
                    result.Add(schedule);
                }
            }
            return result;

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GetAllSchedulesResponse);
        }
    }
}
