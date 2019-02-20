using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    class GetSceneRequest : HueRequestBase
    {
        public string SceneId { get; set; }

        public GetSceneRequest(string id = null) : base("scenes", HttpMethod.Get)
        {
            SceneId = id;
        }

        public override string Address => $"{base.Address}/{SceneId}";

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<GetSceneResponse>(json);
            result.Id = SceneId;
            return result;
        }
    }
}
