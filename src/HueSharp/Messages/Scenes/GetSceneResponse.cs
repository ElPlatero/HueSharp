using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using HueSharp.Converters;
using HueSharp.Messages.Lights;
using System.Linq;

namespace HueSharp.Messages.Scenes
{
    public class GetSceneResponse : IHueResponse
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")] public string Name { get; set; }
        [JsonProperty(PropertyName = "lights")] public IEnumerable<int> LightIds { get; set; }
        [JsonProperty(PropertyName = "owner")] public string Owner { get; set; }
        [JsonProperty(PropertyName = "recycle")] public bool CanRecycle { get; set; }
        [JsonProperty(PropertyName = "locked")] public bool IsLocked { get; set; }
        [JsonProperty(PropertyName = "appdata")] public SceneApplicationData AppData { get; set; }
        [JsonProperty(PropertyName = "picture")] public string Picture { get; set; }
        [JsonProperty(PropertyName = "lastupdated"), JsonConverter(typeof(UtcDateTimeConverter))] public DateTime LastUpdate;
        [JsonProperty(PropertyName = "version"), JsonConverter(typeof(SceneVersionConverter))] public bool LightStatesAvailable { get; set; }

        [JsonProperty(PropertyName = "lightstates"), JsonConverter(typeof(SceneLightStateCollectionConverter))]
        public IEnumerable<SceneLightState> LightStates { get; set; }
        public bool ShouldSerializeLightStates() => LightStates != null && LightStates.Any();
    }
}
