using System;
using System.Drawing;
using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    class SetLightStateRequestBuilder : IModifyLightStateBuilder
    {
        #region fields
        private readonly LightStateBuilder _stateBuilder = new LightStateBuilder();
        private readonly int _lightId;

        #endregion

        /// <summary>
        /// Creates a new request builder for request that alter light states.
        /// </summary>
        /// <param name="lightId">The id of the light as created by the Hue Hub.</param>
        public SetLightStateRequestBuilder(int lightId)
        {
            _lightId = lightId;
        }

        private IModifyLightStateBuilder Perform(Action action)
        {
            action();
            return this;
        }

        /// <inheritdoc />
        public IModifyLightStateBuilder TurnOn() => Perform(_stateBuilder.TurnOn);
        
        /// <inheritdoc />
        public IModifyLightStateBuilder TurnOff() => Perform(_stateBuilder.TurnOff);

        /// <inheritdoc />
        public IModifyLightStateBuilder Brightness(byte brightness) => Perform(() => _stateBuilder.Brightness(brightness));

        /// <inheritdoc />
        public IModifyLightStateBuilder Saturation(byte saturation) => Perform(() => _stateBuilder.Saturation(saturation));

        /// <inheritdoc />
        public IModifyLightStateBuilder Hue(ushort hue) => Perform(() => _stateBuilder.Hue(hue));

        /// <inheritdoc />
        public IModifyLightStateBuilder Color(ushort hue, byte saturation, byte brightness) => Perform(() => _stateBuilder.Color(hue, saturation, brightness));

        public IModifyLightStateBuilder Color(Color color) => Perform(() => _stateBuilder.Color(color));

        /// <inheritdoc />
        public IModifyLightStateBuilder CieLocation(double xCoordinate, double yCoordinate) => Perform(() => _stateBuilder.CieLocation(xCoordinate, yCoordinate));

        /// <inheritdoc />
        public IModifyLightStateBuilder ColorTemperature(ushort miredColorTemperature) => Perform(() => _stateBuilder.ColorTemperature(miredColorTemperature));

        /// <inheritdoc />
        public IModifyLightStateBuilder During(TimeSpan transitionTime) => Perform(() => _stateBuilder.During(transitionTime));

        /// <inheritdoc />
        public IModifyLightStateBuilder ColorLoop() => Perform(_stateBuilder.ColorLoop);

        /// <inheritdoc />
        public IModifyLightStateBuilder Alert(string lightAlert) => Perform(() => _stateBuilder.Alert(lightAlert));

        /// <inheritdoc />
        public ILightStateAccessAdjustRequestBuilder Increase => _stateBuilder.Increase(this);
        /// <inheritdoc />
        public ILightStateAccessAdjustRequestBuilder Decrease => _stateBuilder.Decrease(this);
        /// <inheritdoc />
        public IHueRequest Build() => new SetLightStateRequest(_lightId) {Status = _stateBuilder.BuildState<SetLightState>()};
    }
}