namespace HueSharp.Builder
{
    public interface ILightStateAccessAdjustRequestBuilder
    {
        ILightStateAdjustmentBuilder Brightness { get; }
        ILightStateAdjustmentBuilder Hue { get; }
        ILightStateAdjustmentBuilder Saturation { get; }
    }
}