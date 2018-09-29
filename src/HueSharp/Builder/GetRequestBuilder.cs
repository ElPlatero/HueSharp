namespace HueSharp.Builder
{
    public class GetRequestBuilder
    {
        public IBuilder Lights() => new GetAllLightsRequestBuilder();
        public IBuilder Light(int lightId) => new GetLightStateRequestBuilder(lightId);
    }
}