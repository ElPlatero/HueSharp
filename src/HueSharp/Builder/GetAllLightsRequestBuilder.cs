using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    public class GetAllLightsRequestBuilder : IBuilder
    {
        public IHueRequest Build()
        {
            return new GetAllLightsRequest();
        }
    }
}