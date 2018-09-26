using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    public class GetLightStateRequest : HueRequestBase
    {
        public GetLightStateRequest(int lightId) : base("lights", HttpMethod.Get) { LightId = lightId; }
        public GetLightStateRequest() : base("lights", HttpMethod.Get) { }

        public int LightId { get; set; }

        public override string Address => $"{base.Address}/{LightId}";

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<GetLightStateResponse>(json);
            result.Status.Reset();
            return result;
        }
    }
}
