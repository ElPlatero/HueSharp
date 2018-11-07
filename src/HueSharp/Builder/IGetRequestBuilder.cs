namespace HueSharp.Builder
{
    public interface IGetRequestBuilder
    {
        IBuilder Groups { get; }
        IBuilder Lights { get; }

        IBuilder Group(int groupId);
        IBuilder Light(int lightId);
    }
}