using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    public class SetLightStateRequestBuilder : IModifyLightStateBuilder
    {
        #region fields
        private bool? _isOn;
        private readonly int _lightId;
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
        #endregion

        /// <summary>
        /// Creates a new request builder for request that alter light states.
        /// </summary>
        /// <param name="lightId">The id of the light as created by the Hue Hub.</param>
        public SetLightStateRequestBuilder(int lightId)
        {
            _lightId = lightId;
        }
        private IModifyLightStateBuilder Modify<T>(ref T value, T newValue)
        {
            value = newValue;
            return this;
        }

        /// <summary>
        /// Turns the light on. If <see cref="TurnOff"/> has been called before, it's cancelled out.
        /// </summary>
        public IModifyLightStateBuilder TurnOn()
        {
            if (!_isOn.HasValue) _isOn = true;
            else if (!_isOn.Value) _isOn = null;
            else _isOn = true;
            return this;
        }
        /// <summary>
        /// Turns the light off. If <see cref="TurnOn"/> has been called before, it's cancelled out.
        /// </summary>
        public IModifyLightStateBuilder TurnOff()
        {
            if (!_isOn.HasValue) _isOn = false;
            else if (_isOn.Value) _isOn = null;
            else _isOn = false;
            return this;
        }
        /// <summary>
        /// Sets the brightness of the light. Resets and changes to color temperature and/or alerts that may have been made before.
        /// </summary>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        public IModifyLightStateBuilder Brightness(byte brightness)
        {
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _bri, brightness);
        }
        /// <summary>
        /// Sets the saturation of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        public IModifyLightStateBuilder Saturation(byte saturation)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _sat, saturation);
        }
        /// <summary>
        /// Sets the hue of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        public IModifyLightStateBuilder Hue(ushort hue)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _hue, hue);
        }
        /// <summary>
        /// Sets the color of the light from a HSB-representation. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        public IModifyLightStateBuilder Color(ushort hue, byte saturation, byte brightness) => Hue(hue).Saturation(saturation).Brightness(brightness);

        public IModifyLightStateBuilder Color(Color color)
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

            return CieLocation(x / (x + y + z), y / (x + y + z)).Brightness((byte)(y * byte.MaxValue));
        }


        /// <summary>
        /// Sets the position of the currently displayed light color to the given coordinates in CIE space.
        /// If the gamut of the bulb does not support the given location, the light will switch to the closest point in CIE space it can display.
        /// Resets and changes to color temperature, alerts, hue, and/or saturation that may have been made before.
        /// </summary>
        /// <param name="xCoordinate">A number between 0 and 1.</param>
        /// <param name="yCoordinate">A number between 0 and 1.</param>
        public IModifyLightStateBuilder CieLocation(double xCoordinate, double yCoordinate)
        {
            _hue = _hueInc = _sat = _satInc = _colorTemperature = null;
            _alert = null;
            return Modify(ref _coordinates, new HashSet<double>(new[] {xCoordinate, yCoordinate}));
        }
        /// <summary>
        /// Sets the color temperature as mired color temperature.
        /// Resets and changes to brightness, saturation, hue, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="miredColorTemperature"></param>
        public IModifyLightStateBuilder ColorTemperature(ushort miredColorTemperature)
        {
            _hue = _hueInc = _bri = _briInc = _sat = _satInc = null;
            _coordinates = null;
            _alert = null;
            return Modify(ref _colorTemperature, miredColorTemperature);
        }
        /// <summary>
        /// Sets the transition time for the chosen modifications to the light. If no other modifications have been made, the request will be empty.
        /// </summary>
        /// <param name="transitionTime">The time for the transition as a TimeSpan between 0ms and (theoretically) 6553 seconds.</param>
        public IModifyLightStateBuilder During(TimeSpan transitionTime)
        {
            return Modify(ref _transitionTime, transitionTime);
        }
        /// <summary>
        /// Sets the effect of the lamp to color loop, if supported. Brightness and saturation settings are heeded.
        /// </summary>
        public IModifyLightStateBuilder ColorLoop() => Modify(ref _loop, true);
        /// <summary>
        /// Sets the alert to the given value. Refer to <see cref="LightAlert"/> members for allowed value.
        /// Unknown values will not be included in the built request.
        /// </summary>
        /// <param name="lightAlert">The type of the alert.</param>
        public IModifyLightStateBuilder Alert(string lightAlert)
        {
            _hue = _hueInc = _sat = _satInc = _bri = _briInc = null;
            _colorTemperature = null;
            _coordinates = null;

            return Modify(ref _alert, lightAlert);
        }
        /// <summary>
        /// Allows increment of some basic color values instead of supplying absolute values.
        /// </summary>
        public LightStateAccessAdjustRequestBuilder Increase
        {
            get
            {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value > 65534) value = 65534;
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, true), 
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value > 254) value = 254;
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, true),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
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
        }
        /// <summary>
        /// Allows decrement of some basic color values instead of supplying absolute values.
        /// </summary>
        public LightStateAccessAdjustRequestBuilder Decrease
        {
            get
            {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value < -65534) value = -65534;
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value < -254) value = -254;
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
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

        /// <summary>
        /// Builds the request that can be sent with a <see cref="Net.HueClient"/>.
        /// </summary>
        public IHueRequest Build()
        {
            var result = new SetLightStateRequest(_lightId);
            if (_isOn.HasValue) result.Status.IsOn = _isOn.Value;
            if (_hue.HasValue) result.Status.Hue = _hue.Value;
            if (_sat.HasValue) result.Status.Saturation = _sat.Value;
            if (_bri.HasValue) result.Status.Brightness = _bri.Value;
            if (_coordinates != null) result.Status.Coordinates = _coordinates.ToArray();
            if (_hueInc.HasValue) ((SetLightState) result.Status).HueIncrement = _hueInc.Value;
            if (_satInc.HasValue) ((SetLightState) result.Status).SaturationIncrement = (short) _satInc.Value;
            if (_briInc.HasValue) ((SetLightState) result.Status).BrightnessIncrement = (short) _briInc.Value;
            if (_colorTemperature.HasValue) result.Status.ColorTemperature = _colorTemperature.Value;
            if (_loop.HasValue) result.Status.Effect = _loop.Value ? LightEffect.ColorLoop : LightEffect.None;
            if (_alert == LightAlert.Once || _alert == LightAlert.Cycle) result.Status.Alert = _alert;

            if (result.Status.HasUnsavedChanges && _transitionTime.HasValue) result.Status.TransitionTime = _transitionTime.Value;
            return result;
        }
    }
}