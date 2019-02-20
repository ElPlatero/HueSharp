using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HueSharp.Enums;
using HueSharp.Messages.Groups;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    class LightStateBuilder
    {
        #region fields
        private bool? _isOn;
        private int? _hue;
        private int? _sat;
        private int? _bri;
        private int? _hueInc;
        private int? _satInc;
        private int? _briInc;
        private bool? _loop;
        private ICollection<double> _coordinates;
        private ushort? _colorTemperature;
        private TimeSpan? _transitionTime;
        private string _alert;
        private string _scene;
        #endregion

        public T BuildState<T>() where T : LightState, new()
        {
            var result = new T();

            if (_isOn.HasValue) result.IsOn = _isOn.Value;
            if (_hue.HasValue) result.Hue = _hue.Value;
            if (_sat.HasValue) result.Saturation = _sat.Value;
            if (_bri.HasValue) result.Brightness = _bri.Value;
            if (_coordinates != null) result.Coordinates = _coordinates.ToArray();

            if (result is SetLightState setLightState)
            {
                if (_hueInc.HasValue) setLightState.HueIncrement = _hueInc.Value;
                if (_satInc.HasValue) setLightState.SaturationIncrement = (short) _satInc.Value;
                if (_briInc.HasValue) setLightState.BrightnessIncrement = (short) _briInc.Value;
            }

            if (result is SetGroupState setGroupState)
            {
                if (!string.IsNullOrEmpty(_scene)) setGroupState.Scene = _scene;
            }


            if (_colorTemperature.HasValue) result.ColorTemperature = _colorTemperature.Value;
            if (_loop.HasValue) result.Effect = _loop.Value ? LightEffect.ColorLoop : LightEffect.None;
            if (_alert == LightAlert.Once || _alert == LightAlert.Cycle) result.Alert = _alert;

            if (result.HasUnsavedChanges && _transitionTime.HasValue) result.TransitionTime = _transitionTime.Value;

            return result;
        }


        public void TurnOn()
        {
            if (!_isOn.HasValue) _isOn = true;
            else if (!_isOn.Value) _isOn = null;
            else _isOn = true;
        }
        public void TurnOff()
        {
            if (!_isOn.HasValue) _isOn = false;
            else if (_isOn.Value) _isOn = null;
            else _isOn = false;
        }
        public void Brightness(byte brightness)
        {
            _colorTemperature = null;
            _alert = null;
            _bri = brightness;
        }
        public void Saturation(byte saturation)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            _sat = saturation;
        }
        public void Hue(ushort hue)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            _hue = hue;
        }
        public void Color(ushort hue, byte saturation, byte brightness)
        {
            Hue(hue);
            Saturation(saturation);
            Brightness(brightness);
        }

        public void Color(Color color)
        {
            color = System.Drawing.Color.FromArgb(255, color.R, color.B, color.G);

            // Normalize.
            var red = (float)color.R / 255;
            var green = (float)color.G / 255;
            var blue = (float)color.B / 255;

            // Apply gamma correction.
            red = red > 0.04045f ? (float)Math.Pow((red + 0.055f) / (1.0f + 0.055f), 2.4f) : red / 12.92f;
            green = green > 0.04045f ? (float)Math.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f) : green / 12.92f;
            blue = blue > 0.04045f ? (float)Math.Pow((blue + 0.055f) / (1.0f + 0.055f), 2.4f) : blue / 12.92f;

            // Convert to xyz (RGB D65 formula, wikipedia.)
            var x = red * 0.649926f + green * 0.103455f + blue * 0.197109f;
            var y = red * 0.234327f + green * 0.743075f + blue * 0.022598f;
            var z = red * 0.0000000f + green * 0.053077f + blue * 1.035763f;

            CieLocation(x / (x + y + z), y / (x + y + z));
            Brightness((byte)(y * byte.MaxValue));
        }

        public void CieLocation(double xCoordinate, double yCoordinate)
        {
            _hue = _hueInc = _sat = _satInc = _colorTemperature = null;
            _alert = null;
            _coordinates = new HashSet<double>(new[] { xCoordinate, yCoordinate });
        }

        public void ColorTemperature(ushort miredColorTemperature)
        {
            _hue = _hueInc = _bri = _briInc = _sat = _satInc = null;
            _coordinates = null;
            _alert = null;
            _colorTemperature = miredColorTemperature;
        }

        public void During(TimeSpan transitionTime)
        {
            _transitionTime = transitionTime;
        }

        public void ColorLoop() => _loop = true;

        public void Alert(string lightAlert)
        {
            _hue = _hueInc = _sat = _satInc = _bri = _briInc = null;
            _colorTemperature = null;
            _coordinates = null;

            _alert = lightAlert;
        }

        public void Scene(string scene) => _scene = scene;

        public ILightStateAccessAdjustRequestBuilder Increase(IModifyLightStateBuilder builder)
        {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value > 65534) value = 65534;
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, true),
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value > 254) value = 254;
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, true),
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value > 254) value = 254;
                        if (value != 0)
                        {
                            _bri = null;
                            _briInc = _briInc.HasValue ? _briInc + value : value;
                            if (_briInc == 0) _briInc = null;
                        }
                    }, true));
        }
        /// <summary>
        /// Allows decrement of some basic color values instead of supplying absolute values.
        /// </summary>
        public ILightStateAccessAdjustRequestBuilder Decrease(IModifyLightStateBuilder builder)
        {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value < -65534) value = -65534;
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value < -254) value = -254;
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(builder, value =>
                    {
                        if (value < -254) value = -254;
                        if (value != 0)
                        {
                            _bri = null;
                            _briInc = _briInc.HasValue ? _briInc + value : value;
                            if (_briInc == 0) _briInc = null;
                        }
                    }, false));
        }
    }
}