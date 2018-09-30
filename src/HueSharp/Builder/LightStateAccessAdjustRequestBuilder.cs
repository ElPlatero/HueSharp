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

        /// <summary>
        /// Increment or decrement the current hue.
        /// </summary>
        public SetLightStateAdjustmentRequestBuilder Hue { get; }
        /// <summary>
        /// Increment or decrement the current saturation.
        /// </summary>
        public SetLightStateAdjustmentRequestBuilder Saturation { get; }
        /// <summary>
        /// Increment or decrement the current brightness.
        /// </summary>
        public SetLightStateAdjustmentRequestBuilder Brightness { get; }
    }
}