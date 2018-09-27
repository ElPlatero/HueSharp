using HueSharp.Messages.Schedules;
using Newtonsoft.Json;
using System;

namespace HueSharp.Converters
{
    public class ScheduleTimingConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ScheduleTiming);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(DateTime)) return ScheduleTiming.Create(((DateTime)reader.Value).ToString("s"));
            return ScheduleTiming.Create(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((ScheduleTiming)value).ToJson());
        }
    }
}
