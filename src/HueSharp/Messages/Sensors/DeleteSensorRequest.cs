using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class DeleteSensorRequest : HueRequestBase
    {
        public int SensorId { get; set; }

        public override string Address => $"{base.Address}/{SensorId}";

        public DeleteSensorRequest(int sensorId = -1) : base("sensors", HttpMethod.Delete)
        {
            SensorId = sensorId;
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
