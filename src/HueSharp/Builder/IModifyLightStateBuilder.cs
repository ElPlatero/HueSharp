using System;

namespace HueSharp.Builder
{
    public interface IModifyLightStateBuilder : IBuilder
    {
        IModifyLightStateBuilder TurnOn();
        IModifyLightStateBuilder TurnOff();
        IModifyLightStateBuilder Hue(int hue);
        IModifyLightStateBuilder Saturation(int saturation);
        IModifyLightStateBuilder Brightness(int brightness);
        IModifyLightStateBuilder Color(int hue, int saturation, int brightness);
        IModifyLightStateBuilder CieLocation(double xCoordinate, double yCoordinate);
        IModifyLightStateBuilder ColorTemperature(ushort miredColorTemperature);
        IModifyLightStateBuilder ColorLoop();
        LightStateAccessAdjustRequestBuilder Increase { get; }
        LightStateAccessAdjustRequestBuilder Decrease { get; }
        IModifyLightStateBuilder During(TimeSpan transitionTime);
        IModifyLightStateBuilder Alert(string lightAlert);
    }
}