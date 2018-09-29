using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    public class GetLightStateRequestBuilder : IBuilder
    {
        private readonly int _lightId;

        public GetLightStateRequestBuilder(int lightId)
        {
            _lightId = lightId;
        }
        public IHueRequest Build()
        {
            return new GetLightStateRequest(_lightId);
        }
    }
}