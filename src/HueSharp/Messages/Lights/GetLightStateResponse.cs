using Newtonsoft.Json;

namespace HueSharp.Messages.Lights
{
    public class GetLightStateResponse : IHueResponse
    {
        [JsonProperty(PropertyName = "state")]
        public LightState Status { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "modelid")]
        public string ModelId { get; set; }
        [JsonProperty(PropertyName = "manufacturername")]
        public string Manufacturer { get; set; }
        [JsonProperty(PropertyName = "uniqueid")]
        public string UniqueId { get; set; }
        [JsonProperty(PropertyName = "luminaireuniqueid")]
        public string LuminaireId { get; set; }
        [JsonProperty(PropertyName = "swversion")]
        public string SwVersion { get; set; }
        [JsonProperty(PropertyName = "pointsymbol")]
        public object PointSymbol { get; set; }
    }
}
