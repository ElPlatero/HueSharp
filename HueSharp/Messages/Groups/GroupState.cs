using Newtonsoft.Json;

namespace HueSharp.Messages.Groups
{
    public class GroupState
    {
        [JsonProperty(PropertyName = "all_on")]
        public bool AllOn { get; set; }

        [JsonProperty(PropertyName = "any_on")]
        public bool AnyOn { get; set; }
    }
}
