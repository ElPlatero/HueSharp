using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class GetNewSensorsRequest : HueRequestBase
    {
        public GetNewSensorsRequest() : base("sensors/new", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetNewSensorsResponse>(json);
        }
    }
}
