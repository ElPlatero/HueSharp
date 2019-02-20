using HueSharp.Messages;
using HueSharp.Messages.Scenes;

namespace HueSharp.Builder
{
    class GetSceneRequestBuilder : IBuilder
    {
        private readonly string _sceneId;

        public GetSceneRequestBuilder(string sceneId)
        {
            _sceneId = sceneId;
        }
        public IHueRequest Build() => new GetSceneRequest(_sceneId);
    }
}