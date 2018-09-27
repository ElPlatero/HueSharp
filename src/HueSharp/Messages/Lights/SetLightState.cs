using Newtonsoft.Json;
using System;

namespace HueSharp.Messages.Lights
{
    public class SetLightState : LightState
    {
        private short _incBrightness;
        private short _incSaturation;
        private int _incHue;
        private int _incColorTemperature;
        private double[] _incCoordinates;

        [JsonProperty(PropertyName = "bri_inc")]
        public short BrightnessIncrement
        {
            get { return _incBrightness; }
            set
            {
                if (value < -254 || value > 255) throw new ArgumentOutOfRangeException($"Brightness increments must not be less than -254 or greater than 255.");
                SetValue(ref _incBrightness, value);
            }
        }
        public bool ShouldSerializeBrightnessIncrement() => !ShouldSerialize(nameof(Brightness)) && ShouldSerialize(nameof(BrightnessIncrement));

        [JsonProperty(PropertyName = "sat_inc")]
        public short SaturationIncrement
        {
            get { return _incSaturation; }
            set
            {
                if (value < -254 || value > 255) throw new ArgumentOutOfRangeException($"Saturation increments must not be less than -254 or greater than 255.");
                SetValue(ref _incSaturation, value);
            }
        }
        public bool ShouldSerializeSaturationIncrement() => !ShouldSerialize(nameof(Saturation)) && ShouldSerialize(nameof(SaturationIncrement));

        [JsonProperty(PropertyName = "hue_inc")]
        public int HueIncrement
        {
            get { return _incHue; }
            set
            {
                if (value < -65534 || value > 65535) throw new ArgumentOutOfRangeException($"Hue increments must not be less than -65534 or greater than 65535.");
                SetValue(ref _incHue, value);
            }
        }
        public bool ShouldSerializeHueIncrement() => !ShouldSerialize(nameof(Hue)) && ShouldSerialize(nameof(HueIncrement));

        [JsonProperty(PropertyName = "ct_inc")]
        public int ColorTemperatureIncrement
        {
            get { return _incColorTemperature; }
            set
            {
                if (value < -65534 || value > 65535) throw new ArgumentOutOfRangeException($"Color temperature increments must not be less than -65534 or greater than 65535.");
                SetValue(ref _incColorTemperature, value);
            }
        }
        public bool ShouldSerializeColorTemperatureIncrement() => !ShouldSerialize(nameof(ColorTemperature)) && ShouldSerialize(nameof(ColorTemperatureIncrement));

        [JsonProperty(PropertyName = "xy_inc")]
        public double[] CoordinatesIncrement
        {
            get { return _incCoordinates; }
            set
            {
                if (value.Length != 2) throw new ArgumentOutOfRangeException($"The set of coordinates to change the current CIE color coordinates must be a size of 2.");
                if (value[0] < -0.5 || value[0] > 0.5) throw new ArgumentOutOfRangeException($"CIE X coordinate increment must not be less than -0.5 or greater than 0.5.");
                if (value[1] < -0.5 || value[1] > 0.5) throw new ArgumentOutOfRangeException($"CIE Y coordinate increment must not be less than -0.5 or greater than 0.5.");
                SetValue(ref _incCoordinates, value);
            }
        }
        public bool ShouldSerializeCoordinatesIncrement() => !ShouldSerialize(nameof(Coordinates)) && ShouldSerialize(nameof(CoordinatesIncrement));

    }
}
