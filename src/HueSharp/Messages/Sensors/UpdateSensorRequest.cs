using HueSharp.Converters;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class UpdateSensorRequest : HueRequestBase, IUploadable
    {
        public int SensorId { get; set; }
        public string NewName { get; set; }
        public override string Address => $"{base.Address}/{SensorId}";

        public UpdateSensorRequest(int sensorId = -1, string newName = null) : base("sensors", HttpMethod.Put) { SensorId = sensorId; NewName = newName; }

        public string GetRequestBody()
        {
            using(var sw = new StringWriter())
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(NewName);
                writer.WriteEndObject();
                return sw.ToString();
            }
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
