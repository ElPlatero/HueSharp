using HueSharp.Messages;
using HueSharp.Messages.Scenes;

namespace HueSharp.Builder
{
    class GetAllScenesRequestBuilder : IBuilder
    {
        public IHueRequest Build() => new GetAllScenesRequest();
    }
}