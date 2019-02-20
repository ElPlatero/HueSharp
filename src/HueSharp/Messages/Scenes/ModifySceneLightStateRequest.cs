using HueSharp.Converters;
using HueSharp.Messages.Lights;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    class ModifySceneLightStateRequest : HueRequestBase, IUploadable
    {
        public ModifySceneLightStateRequest(string sceneId = null, int lightId = 0) : base("scenes", HttpMethod.Put)
        {
            SceneId = sceneId;
            LightId = lightId;
        }

        public string SceneId { get; set; }
        public int LightId { get; set; }
        public LightState LightState { get; set; }

        public override string Address => $"{base.Address}/{SceneId}/lightstates/{LightId}";

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(LightState);
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
