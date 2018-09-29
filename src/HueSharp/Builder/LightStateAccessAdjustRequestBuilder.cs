namespace HueSharp.Builder
{
    public class LightStateAccessAdjustRequestBuilder
    {
        public LightStateAccessAdjustRequestBuilder(SetLightStateAdjustmentRequestBuilder hue, SetLightStateAdjustmentRequestBuilder saturation, SetLightStateAdjustmentRequestBuilder brightness)
        {
            Brightness = brightness;
            Hue = hue;
            Saturation = saturation;
        }

        public SetLightStateAdjustmentRequestBuilder Hue { get; }
        public SetLightStateAdjustmentRequestBuilder Saturation { get; }
        public SetLightStateAdjustmentRequestBuilder Brightness { get; }
    }
}