using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    public class DeleteSceneRequest : HueRequestBase
    {
        public string SceneId { get; set; }

        public DeleteSceneRequest(string id = null) : base("scenes", HttpMethod.Delete)
        {
            SceneId = id;
        }

        public override string Address => $"{base.Address}/{SceneId}";

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
