using HueSharp.Messages;

namespace HueSharp.Builder
{
    class SetRequestBuilder : ISetRequestBuilder
    {
        public IModifyLightBuilder Light(int lightId) => new ModifyLightBuilder(lightId);
        public IModifyGroupBuilder Group(int groupId) => new ModifyGroupBuilder(groupId);
        public IModifyGroupBuilder Group(IHueResponse fromResponse) => new ModifyGroupBuilder(fromResponse);
    }
}