namespace HueSharp.Builder
{
    public interface IGetRequestBuilder
    {
        IBuilder Groups { get; }
        IBuilder Lights { get; }
        IBuilder Scenes { get; }

        IBuilder Group(int groupId);
        IBuilder Light(int lightId);
        IBuilder Scene(string sceneId);
    }
}