using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class GetAllSensorsRequest : HueRequestBase
    {
        public GetAllSensorsRequest() : base("sensors", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetAllSensorsResponse>(json, new SensorBaseConverter());
        }
    }
}
