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

        public IModifyLightStateBuilder Brightness(int brightness) => Modify(ref _bri, brightness);
        public IModifyLightStateBuilder Saturation(int saturation) => Modify(ref _sat, saturation);
        public IModifyLightStateBuilder Hue(int hue) => Modify(ref _hue, hue);
        public IModifyLightStateBuilder Color(int hue, int saturation, int brightness) => Hue(hue).Saturation(saturation).Brightness(brightness);
        public IModifyLightStateBuilder ColorLoop() => Modify(ref _loop, true);
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
            if (_hueInc.HasValue) ((SetLightState)result.Status).HueIncrement = (short)_hueInc.Value;
            if (_satInc.HasValue) ((SetLightState)result.Status).SaturationIncrement = (short)_satInc.Value;
            if (_briInc.HasValue) ((SetLightState)result.Status).BrightnessIncrement = (short)_briInc.Value;
            if (_loop.HasValue) result.Status.Effect = _loop.Value ? LightEffect.ColorLoop : LightEffect.None;
            return result;
        }
    }
}