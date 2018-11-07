using HueSharp.Converters;
using HueSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HueSharp.Messages.Lights
{
    public class LightState
    {
        #region private members
        protected Dictionary<string, bool> _shouldSerialize = new Dictionary<string, bool>();
        private bool _isOn;
        private int _brightness;
        private int _hue;
        private int _saturation;
        private LightEffect _effect;
        private double[] _coordinates;
        private UInt16 _colorTemperature;
        private string _alert;
        private string _colorMode;
        protected TimeSpan _transitionTime;
        #endregion

        #region Properties
        [JsonProperty(PropertyName = "on")]
        public bool IsOn { get { return _isOn; } set { SetValue(ref _isOn, value); } }
        public bool ShouldSerializeIsOn() => ShouldSerialize(nameof(IsOn));

        [JsonProperty(PropertyName = "bri")]
        public int Brightness { get { return _brightness; } set { SetValue(ref _brightness, value); } }
        public bool ShouldSerializeBrightness() => ShouldSerialize(nameof(Brightness));

        [JsonProperty(PropertyName = "hue")]
        public int Hue { get { return _hue; } set { SetValue(ref _hue, value); } }
        public bool ShouldSerializeHue() => ShouldSerialize(nameof(Hue));

        [JsonProperty(PropertyName = "sat")]
        public int Saturation { get { return _saturation; } set { SetValue(ref _saturation, value); } }
        public bool ShouldSerializeSaturation() => ShouldSerialize(nameof(Saturation));

        [JsonProperty(PropertyName = "effect"), JsonConverter(typeof(DescriptionConverter))]
        public LightEffect Effect { get { return _effect; } set { SetValue(ref _effect, value); } }
        public bool ShouldSerializeEffect() => ShouldSerialize(nameof(Effect));

        [JsonProperty(PropertyName = "xy")]
        public double[] Coordinates { get { return _coordinates; } set { SetValue(ref _coordinates, value); } }
        public bool ShouldSerializeCoordinates() => ShouldSerialize(nameof(Coordinates));

        public UInt16 ColorTemperature {  get { return _colorTemperature; } set { SetValue(ref _colorTemperature, value); } }
        public bool ShouldSerializeColorTemperature() => ShouldSerialize(nameof(ColorTemperature));

        [JsonProperty(PropertyName = "alert")]
        public string Alert { get { return _alert; } set { SetValue(ref _alert, value); } }
        public bool ShouldSerializeAlert() => ShouldSerialize(nameof(Alert));

        [JsonProperty(PropertyName = "colormode")]
        public string ColorMode { get { return _colorMode; } set { SetValue(ref _colorMode, value); } }
        public bool ShouldSerializeColorMode() => ShouldSerialize(nameof(ColorMode));

        [JsonProperty(PropertyName = "reachable")]
        public bool IsReachable { get; set; }
        public bool ShouldSerializeIsReachable() { return false; }

        [JsonProperty(PropertyName = "transitiontime")]
        private UInt16 TransitionTimeAsNumber
        {
            get { return Convert.ToUInt16(_transitionTime.TotalSeconds * 10); }
            set { SetValue(ref _transitionTime, TimeSpan.FromSeconds(Convert.ToInt32(value / 10))); }
        }
        public bool ShouldSerializeTransitionTimeAsNumber() => ShouldSerialize(nameof(TransitionTimeAsNumber));

        [JsonIgnore]
        // ReSharper disable once ExplicitCallerInfoArgument
        public TimeSpan TransitionTime { get { return _transitionTime; } set { SetValue(ref _transitionTime, value, "TransitionTimeAsNumber"); } }



        [JsonIgnore]
        public bool HasUnsavedChanges {  get { return _shouldSerialize.Any(); } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var modified in _shouldSerialize.Keys)
            {
                if (!_shouldSerialize[modified]) continue;
                var value = GetType().GetProperty(modified).GetValue(this);
                sb.AppendLine($"\t{modified} : {value}");
            }
            return sb.ToString();
        }

        #endregion

        #region property tracking
        protected void SetValue<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            if (!_shouldSerialize.ContainsKey(propertyName)) _shouldSerialize.Add(propertyName, true);
            else _shouldSerialize[propertyName] = true;

            field = newValue;
        }
        #endregion

        #region utils
        protected bool ShouldSerialize(string propertyName)
        {
            return _shouldSerialize.ContainsKey(propertyName) && _shouldSerialize[propertyName];
        }
        public void Reset()
        {
            _shouldSerialize.Clear();
        }

        public void ApplyChanges(Dictionary<string, object> changedProperties)
        {
            foreach(var propertyInfo in GetType().GetProperties())
            {
                if (!HasUnsavedChanges) return;
                if (!_shouldSerialize.ContainsKey(propertyInfo.Name)) continue;

                var attributes = (JsonPropertyAttribute[])propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                if (attributes == null || attributes.Length == 0 || !changedProperties.ContainsKey(attributes[0].PropertyName)) continue;

                _shouldSerialize.Remove(propertyInfo.Name);
            }
        }
        #endregion
    }
}
