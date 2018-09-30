using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    class GetAllLightsRequest : HueRequestBase
    {
        public GetAllLightsRequest() : base("lights", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<GetAllLightsResponse>(json);
            result.ResetAllStates();
            return result;
        }
    }
}
