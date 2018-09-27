using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class FindNewSensorsRequest : HueRequestBase
    {
        public FindNewSensorsRequest() : base("sensors", HttpMethod.Post) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
