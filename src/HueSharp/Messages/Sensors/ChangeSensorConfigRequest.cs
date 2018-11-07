using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class ChangeSensorConfigRequest : HueRequestBase, IUploadable
    {
        public SensorBase Sensor { get; set; }

        public override string Address => $"{base.Address}/{Sensor.Id}/config";


        public ChangeSensorConfigRequest(SensorBase sensor = null) : base("sensors", HttpMethod.Put)
        {
            Sensor = sensor;
        }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(Sensor.Configuration);
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
