using HueSharp.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HueSharp.Messages.Scenes
{
    public class ModifySceneParameters
    {
        [JsonIgnore]
        public string SceneId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [JsonProperty(PropertyName = "lights"), JsonConverter(typeof(IntAsStringConverter))]
        public IEnumerable<int> LightIds { get; set; } = new List<int>();
        public bool ShouldSerializeLightIds() => LightIds.Any();

        [JsonProperty(PropertyName = "storelightstate")]
        public bool UseCurrentStatus { get; set; }
        public bool ShouldSerializeUseCurrentStatus() => UseCurrentStatus;
    }
}
