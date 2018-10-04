using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    class GetGroupStateRequestBuilder : IBuilder
    {
        private readonly int _groupId;

        public GetGroupStateRequestBuilder(int groupId)
        {
            _groupId = groupId;
        }

        public IHueRequest Build()
            => new GetGroupRequest(_groupId);
    }
}
