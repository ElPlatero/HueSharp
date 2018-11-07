using Newtonsoft.Json;

namespace HueSharp.Messages.Sensors
{
    public class BasicSensor
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
