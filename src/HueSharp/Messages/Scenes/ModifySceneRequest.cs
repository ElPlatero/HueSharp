using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    public class ModifySceneRequest : HueRequestBase, IUploadable
    {
        public ModifySceneParameters Parameters { get; set; }

        public ModifySceneRequest(ModifySceneParameters parameters = null) : base("scenes", HttpMethod.Put)
        {
            Parameters = parameters;
        }

        public override string Address => $"{base.Address}/{Parameters.SceneId}";

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(Parameters);
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
