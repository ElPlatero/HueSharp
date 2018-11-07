namespace HueSharp.Builder
{
    class LightStateAccessAdjustRequestBuilder : ILightStateAccessAdjustRequestBuilder
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
        public ILightStateAdjustmentBuilder Hue { get; }
        /// <summary>
        /// Increment or decrement the current saturation.
        /// </summary>
        public ILightStateAdjustmentBuilder Saturation { get; }
        /// <summary>
        /// Increment or decrement the current brightness.
        /// </summary>
        public ILightStateAdjustmentBuilder Brightness { get; }
    }
}