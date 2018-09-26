using Newtonsoft.Json;

namespace HueSharp.Messages.Lights
{
    public class Light : BasicLight
    {
        [JsonProperty(PropertyName = "state")]
        public LightState Status { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
