using Newtonsoft.Json;

namespace HueSharp.Messages.Lights
{
    public class BasicLight
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public int Id { get; set; }

    }
}