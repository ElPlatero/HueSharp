using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    public class SearchNewLightsRequest : HueRequestBase
    {
        public SearchNewLightsRequest() : base("lights", HttpMethod.Post) { }

        protected override IHueResponse Deserialize(string json)
        {
            var successMessage = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            return successMessage;
        }
    }
}
