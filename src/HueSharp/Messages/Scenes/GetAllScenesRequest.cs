using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    public class GetAllScenesRequest : HueRequestBase
    {
        public GetAllScenesRequest() : base("scenes", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetAllScenesResponse>(json);
        }
    }
}
