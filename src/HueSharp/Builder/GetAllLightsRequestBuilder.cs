using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    class GetAllLightsRequestBuilder : IBuilder
    {
        public IHueRequest Build()
        {
            return new GetAllLightsRequest();
        }
    }
}