using HueSharp.Messages;

namespace HueSharp.Builder
{
    public interface ISetRequestBuilder
    {
        IModifyLightBuilder Light(int lightId);
        IModifyGroupBuilder Group(int groupId);
        IModifyGroupBuilder Group(IHueResponse fromResponse);
    }
}