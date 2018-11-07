using HueSharp.Enums;
using Newtonsoft.Json;

namespace HueSharp.Messages
{
    public class ErrorMessage
    {
        [JsonProperty(PropertyName = "type")]
        public ErrorCode Type { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
