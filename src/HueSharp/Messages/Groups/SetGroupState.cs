using HueSharp.Messages.Lights;
using Newtonsoft.Json;

namespace HueSharp.Messages.Groups
{
    public class SetGroupState : LightState
    {
        private string _scene;

        [JsonProperty(PropertyName = "scene")]
        public string Scene { get { return _scene; } set { SetValue(ref _scene, value); } }
        public bool ShouldSerializeScene() => ShouldSerialize(nameof(Scene));
    }
}
