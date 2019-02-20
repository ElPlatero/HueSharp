using System;
using System.Drawing;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface IModifyGroupStateBuilder : IBuilder
    {
        /// <summary>
        /// Turns the light on. If <see cref="TurnOff"/> has been called before, it's cancelled out.
        /// </summary>
        IModifyGroupStateBuilder TurnOn();
        /// <summary>
        /// Turns the light off. If <see cref="TurnOn"/> has been called before, it's cancelled out.
        /// </summary>
        IModifyGroupStateBuilder TurnOff();
        /// <summary>
        /// Sets the hue of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        IModifyGroupStateBuilder Hue(ushort hue);
        /// <summary>
        /// Sets the saturation of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        IModifyGroupStateBuilder Saturation(byte saturation);
        /// <summary>
        /// Sets the brightness of the light. Resets and changes to color temperature and alerts that may have been made before.
        /// </summary>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        IModifyGroupStateBuilder Brightness(byte brightness);
        /// <summary>
        /// Sets the color of the light from a HSB-representation. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        IModifyGroupStateBuilder Color(ushort hue, byte saturation, byte brightness);
        /// <summary>
        /// Sets the color of the light from a RGB-representation. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        IModifyGroupStateBuilder Color(Color color);
        /// <summary>
        /// Sets the position of the currently displayed light color to the given coordinates in CIE space.
        /// If the gamut of the bulb does not support the given location, the light will switch to the closest point in CIE space it can display.
        /// Resets and changes to color temperature, alerts, hue, and/or saturation that may have been made before.
        /// </summary>
        /// <param name="xCoordinate">A number between 0 and 1.</param>
        /// <param name="yCoordinate">A number between 0 and 1.</param>
        IModifyGroupStateBuilder CieLocation(double xCoordinate, double yCoordinate);
        /// <summary>
        /// Sets the color temperature as mired color temperature.
        /// Resets and changes to brightness, saturation, hue, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="miredColorTemperature"></param>
        IModifyGroupStateBuilder ColorTemperature(ushort miredColorTemperature);
        /// <summary>
        /// Sets the effect of the lamp to color loop, if supported. Brightness and saturation settings are heeded.
        /// </summary>
        IModifyGroupStateBuilder ColorLoop();
        /// <summary>
        /// Sets the transition time for the chosen modifications to the light. If no other modifications have been made, the request will be empty.
        /// </summary>
        /// <param name="transitionTime">The time for the transition as a TimeSpan between 0ms and (theoretically) 6553 seconds.</param>
        IModifyGroupStateBuilder During(TimeSpan transitionTime);
        /// <summary>
        /// Sets the alert to the given value. Refer to <see cref="Messages.Lights.LightAlert"/> members for allowed value.
        /// Unknown values will not be included in the built request.
        /// </summary>
        /// <param name="lightAlert">The type of the alert.</param>
        IModifyGroupStateBuilder Alert(string lightAlert);
        /// <summary>
        /// Sets the scene for the current group.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        IModifyGroupStateBuilder Scene(string scene);
    }

    class ModifyGroupStateBuilder : IModifyGroupStateBuilder
    {
        private readonly int _groupId;
        private readonly LightStateBuilder _stateBuilder = new LightStateBuilder();

        public ModifyGroupStateBuilder(int groupId)
        {
            _groupId = groupId;
        }

        private IModifyGroupStateBuilder Perform(Action action)
        {
            action();
            return this;
        }

        public IHueRequest Build() => new SetGroupStateRequest(_groupId, _stateBuilder.BuildState<SetGroupState>());

        public IModifyGroupStateBuilder TurnOn() => Perform(_stateBuilder.TurnOn);
        public IModifyGroupStateBuilder TurnOff() => Perform(_stateBuilder.TurnOff);
        public IModifyGroupStateBuilder Hue(ushort hue) => Perform(( )=> _stateBuilder.Hue(hue));
        public IModifyGroupStateBuilder Saturation(byte saturation) => Perform(() => _stateBuilder.Saturation(saturation));
        public IModifyGroupStateBuilder Brightness(byte brightness) => Perform(() => _stateBuilder.Brightness(brightness));
        public IModifyGroupStateBuilder Color(ushort hue, byte saturation, byte brightness) => Perform(() => _stateBuilder.Color(hue, saturation, brightness));
        public IModifyGroupStateBuilder Color(Color color) => Perform(() => _stateBuilder.Color(color));
        public IModifyGroupStateBuilder CieLocation(double xCoordinate, double yCoordinate) => Perform(() => _stateBuilder.CieLocation(xCoordinate, yCoordinate));
        public IModifyGroupStateBuilder ColorTemperature(ushort miredColorTemperature) => Perform(() => _stateBuilder.ColorTemperature(miredColorTemperature));
        public IModifyGroupStateBuilder ColorLoop() => Perform(_stateBuilder.ColorLoop);
        public IModifyGroupStateBuilder During(TimeSpan transitionTime) => Perform(() => _stateBuilder.During(transitionTime));
        public IModifyGroupStateBuilder Alert(string lightAlert) => Perform(() => _stateBuilder.Alert(lightAlert));
        public IModifyGroupStateBuilder Scene(string scene) => Perform(() => _stateBuilder.Scene(scene));
    }
}