using System;
using System.Drawing;

namespace HueSharp.Builder
{
    public interface IModifyLightStateBuilder : IBuilder
    {
        /// <summary>
        /// Turns the light on. If <see cref="TurnOff"/> has been called before, it's cancelled out.
        /// </summary>
        IModifyLightStateBuilder TurnOn();
        /// <summary>
        /// Turns the light off. If <see cref="TurnOn"/> has been called before, it's cancelled out.
        /// </summary>
        IModifyLightStateBuilder TurnOff();
        /// <summary>
        /// Sets the hue of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        IModifyLightStateBuilder Hue(ushort hue);
        /// <summary>
        /// Sets the saturation of the light. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        IModifyLightStateBuilder Saturation(byte saturation);
        /// <summary>
        /// Sets the brightness of the light. Resets and changes to color temperature and alerts that may have been made before.
        /// </summary>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        IModifyLightStateBuilder Brightness(byte brightness);
        /// <summary>
        /// Sets the color of the light from a HSB-representation. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="hue">The hue as a number between 1 and 65535.</param>
        /// <param name="saturation">The saturation as a number between 1 and 254.</param>
        /// <param name="brightness">The brightness as a number between 1 and 254.</param>
        IModifyLightStateBuilder Color(ushort hue, byte saturation, byte brightness);
        /// <summary>
        /// Sets the color of the light from a RGB-representation. Resets and changes to color temperature, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        IModifyLightStateBuilder Color(Color color);
        /// <summary>
        /// Sets the position of the currently displayed light color to the given coordinates in CIE space.
        /// If the gamut of the bulb does not support the given location, the light will switch to the closest point in CIE space it can display.
        /// Resets and changes to color temperature, alerts, hue, and/or saturation that may have been made before.
        /// </summary>
        /// <param name="xCoordinate">A number between 0 and 1.</param>
        /// <param name="yCoordinate">A number between 0 and 1.</param>
        IModifyLightStateBuilder CieLocation(double xCoordinate, double yCoordinate);
        /// <summary>
        /// Sets the color temperature as mired color temperature.
        /// Resets and changes to brightness, saturation, hue, alerts and/or CIE coordinates that may have been made before.
        /// </summary>
        /// <param name="miredColorTemperature"></param>
        IModifyLightStateBuilder ColorTemperature(ushort miredColorTemperature);
        /// <summary>
        /// Sets the effect of the lamp to color loop, if supported. Brightness and saturation settings are heeded.
        /// </summary>
        IModifyLightStateBuilder ColorLoop();
        /// <summary>
        /// Allows increment of some basic color values instead of supplying absolute values.
        /// </summary>
        ILightStateAccessAdjustRequestBuilder Increase { get; }
        /// <summary>
        /// Allows decrement of some basic color values instead of supplying absolute values.
        /// </summary>
        ILightStateAccessAdjustRequestBuilder Decrease { get; }
        /// <summary>
        /// Sets the transition time for the chosen modifications to the light. If no other modifications have been made, the request will be empty.
        /// </summary>
        /// <param name="transitionTime">The time for the transition as a TimeSpan between 0ms and (theoretically) 6553 seconds.</param>
        IModifyLightStateBuilder During(TimeSpan transitionTime);
        /// <summary>
        /// Sets the alert to the given value. Refer to <see cref="Messages.Lights.LightAlert"/> members for allowed value.
        /// Unknown values will not be included in the built request.
        /// </summary>
        /// <param name="lightAlert">The type of the alert.</param>
        IModifyLightStateBuilder Alert(string lightAlert);
    }
}