using HueSharp.Enums;
using HueSharp.Messages.Lights;
using Newtonsoft.Json;

namespace HueSharp.Messages.Groups
{
    public class GetGroupResponse : IHueResponse, IHueStatusMessage
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lights")]
        public int[] LightIds { get; set; }
        [JsonProperty(PropertyName = "type")]
        public GroupType Type { get; set; }
        [JsonProperty(PropertyName = "state")]
        public GroupState State { get; set; }
        [JsonProperty(PropertyName = "class")]
        public RoomClass Class { get; set; }
        [JsonProperty(PropertyName = "recycle")]
        public bool IsRecycle { get; set; }
        [JsonProperty(PropertyName = "action")]
        public LightState Action { get; set; }
        [JsonIgnore]
        public LightState Status
        {
            get => Action;
            set => Action = value;
        }
    }
}
