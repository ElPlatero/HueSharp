using System;
using System.Collections.Generic;
using System.Linq;
using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    public class SetLightStateRequestBuilder : IModifyLightStateBuilder
    {
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
        public SetLightStateRequestBuilder(int lightId)
        {
            _lightId = lightId;
        }

        private IModifyLightStateBuilder Modify<T>(ref T value, T newValue)
        {
            value = newValue;
            return this;
        }

        public IModifyLightStateBuilder TurnOn()
        {
            if (!_isOn.HasValue) _isOn = true;
            else if (!_isOn.Value) _isOn = null;
            else _isOn = true;
            return this;
        }
        public IModifyLightStateBuilder TurnOff()
        {
            if (!_isOn.HasValue) _isOn = false;
            else if (_isOn.Value) _isOn = null;
            else _isOn = false;
            return this;
        }

        public IModifyLightStateBuilder Brightness(int brightness)
        {
            _coordinates =  null;
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _bri, brightness);
        }

        public IModifyLightStateBuilder Saturation(int saturation)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _sat, saturation);
        }

        public IModifyLightStateBuilder Hue(int hue)
        {
            _coordinates = null;
            _colorTemperature = null;
            _alert = null;
            return Modify(ref _hue, hue);
        }

        public IModifyLightStateBuilder Color(int hue, int saturation, int brightness) => Hue(hue).Saturation(saturation).Brightness(brightness);
        public IModifyLightStateBuilder CieLocation(double xCoordinate, double yCoordinate)
        {
            _hue = _hueInc = _bri = _briInc = _sat = _satInc = _colorTemperature = null;
            _alert = null;
            return Modify(ref _coordinates, new HashSet<double>(new[] {xCoordinate, yCoordinate}));
        }

        public IModifyLightStateBuilder ColorTemperature(ushort miredColorTemperature)
        {
            _hue = _hueInc = _bri = _briInc = _sat = _satInc = null;
            _coordinates = null;
            _alert = null;
            return Modify(ref _colorTemperature, miredColorTemperature);
        }

        public IModifyLightStateBuilder During(TimeSpan transitionTime)
        {
            return Modify(ref _transitionTime, transitionTime);
        }

        public IModifyLightStateBuilder ColorLoop() => Modify(ref _loop, true);

        public IModifyLightStateBuilder Alert(string lightAlert)
        {
            _hue = _hueInc = _sat = _satInc = _bri = _briInc = null;
            _colorTemperature = null;
            _coordinates = null;

            return Modify(ref _alert, lightAlert);
        }
        public LightStateAccessAdjustRequestBuilder Increase
        {
            get
            {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, true), 
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, true),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _bri = null;
                            _briInc = _briInc.HasValue ? _briInc + value : value;
                            if (_briInc == 0) _briInc = null;
                        }
                    }, true));
            }
        }
        public LightStateAccessAdjustRequestBuilder Decrease
        {
            get
            {
                return new LightStateAccessAdjustRequestBuilder(
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _hue = null;
                            _hueInc = _hueInc.HasValue ? _hueInc + value : value;
                            if (_hueInc == 0) _hueInc = null;
                        }

                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _sat = null;
                            _satInc = _satInc.HasValue ? _satInc + value : value;
                            if (_satInc == 0) _satInc = null;
                        }
                    }, false),
                    new SetLightStateAdjustmentRequestBuilder(this, value =>
                    {
                        if (value != 0)
                        {
                            _bri = null;
                            _briInc = _briInc.HasValue ? _briInc + value : value;
                            if (_briInc == 0) _briInc = null;
                        }
                    }, false));
            }
        }

        public IHueRequest Build()
        {
            var result = new SetLightStateRequest(_lightId);
            if (_isOn.HasValue) result.Status.IsOn = _isOn.Value;
            if (_hue.HasValue) result.Status.Hue = _hue.Value;
            if (_sat.HasValue) result.Status.Saturation = _sat.Value;
            if (_bri.HasValue) result.Status.Brightness = _bri.Value;
            if (_coordinates != null) result.Status.Coordinates = _coordinates.ToArray();
            if (_hueInc.HasValue) ((SetLightState) result.Status).HueIncrement = (short) _hueInc.Value;
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