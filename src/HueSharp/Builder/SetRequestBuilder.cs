namespace HueSharp.Builder
{
    public class SetRequestBuilder
    {
        public IModifyLightStateBuilder Light(int lightId) => new SetLightStateRequestBuilder(lightId);
    }
}