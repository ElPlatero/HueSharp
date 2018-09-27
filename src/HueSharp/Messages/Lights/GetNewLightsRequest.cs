using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    public class GetNewLightsRequest : HueRequestBase
    {
        public GetNewLightsRequest() : base("lights/new", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetNewLightsResponse>(json);
        }
    }
}
