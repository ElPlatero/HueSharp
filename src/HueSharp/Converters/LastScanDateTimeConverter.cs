using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Converters
{
    class LastScanDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.ToString().Equals("none")) return DateTime.MinValue;
            else if (reader.Value.ToString().Equals("active")) return DateTime.MaxValue;
            else return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
