using Newtonsoft.Json;

namespace HueSharp.Messages.Scenes
{
    public class SceneApplicationData
    {
        [JsonProperty(PropertyName = "version")]
        public byte Version { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
}
