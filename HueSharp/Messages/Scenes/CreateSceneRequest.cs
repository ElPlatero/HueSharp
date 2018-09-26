using HueSharp.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Scenes
{
    public class CreateSceneRequest : HueRequestBase, IUploadable
    {
        public CreateSceneParameters Parameters { get; set; }

        public CreateSceneRequest() : base("scenes", HttpMethod.Post) { }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(Parameters);
        }

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            Parameters.SceneId = result["id"].ToString();
            return result;
        }
    }
}
