using System.Net.Http;
using Newtonsoft.Json;

namespace HueSharp.Messages.Schedules
{
    public class GetAllSchedulesRequest : HueRequestBase
    {
        public GetAllSchedulesRequest() : base("schedules", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetAllSchedulesResponse>(json);
        }
    }
}
