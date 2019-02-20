namespace HueSharp.Builder
{
    public interface ICreateRequestBuilder
    {
        ICreateGroupInitBuilder Group { get; }
        CreateSceneInitBuilder Scene { get; }
    }

    class CreateRequestBuilder : ICreateRequestBuilder
    {
        public ICreateGroupInitBuilder Group => new CreateGroupInitBuilder();
        public CreateSceneInitBuilder Scene => new CreateSceneInitBuilder();
    }

}