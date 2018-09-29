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
        IModifyLightStateBuilder ColorLoop();
        LightStateAccessAdjustRequestBuilder Increase { get; }
        LightStateAccessAdjustRequestBuilder Decrease { get; }
    }
}