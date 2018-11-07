using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    class GetAllGroupsRequestBuilder : IBuilder
    {
        public IHueRequest Build()
        {
            return new GetAllGroupsRequest();
        }
    }
}
