using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HueSharp.Messages.Scenes
{
    public class CreateSceneParameters
    {
        [JsonIgnore]
        public string SceneId { get; set; }

        [JsonProperty(PropertyName = "lights"), JsonConverter(typeof(IntAsStringConverter))]
        public IEnumerable<int> LightIds { get; set; } = new List<int>();

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "recycle")]
        public bool IsRecycle { get; set; }

        [JsonProperty(PropertyName = "appdata")]
        public SceneApplicationData AppData { get; set; }
        public bool ShouldSerializeAppData() => AppData != null;

        [JsonProperty(PropertyName = "picture")]
        public string Picture { get; set; }
        public bool ShouldSerializePicture() => !string.IsNullOrEmpty(Picture);

        [JsonIgnore]
        public TimeSpan TransitionTime { get; set; }

        [JsonProperty(PropertyName = "transitiontime")]
        private UInt16 TransitionTimeAsNumber
        {
            get => Convert.ToUInt16(TransitionTime.TotalSeconds * 10);
            set => TransitionTime = TimeSpan.FromSeconds(Convert.ToInt32(value / 10));
        }
        public bool ShouldSerializeTransitionTimeAsNumber() => TransitionTime.TotalSeconds > 0;
    }
}
