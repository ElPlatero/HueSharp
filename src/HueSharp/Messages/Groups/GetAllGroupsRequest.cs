using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    class GetAllGroupsRequest : HueRequestBase
    {
        public GetAllGroupsRequest() : base("groups", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetAllGroupsResponse>(json);
        }
    }
}
