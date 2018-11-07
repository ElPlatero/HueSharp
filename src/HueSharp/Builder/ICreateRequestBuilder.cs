namespace HueSharp.Builder
{
    public interface ICreateRequestBuilder
    {
        ICreateGroupInitBuilder Group { get; }
    }

    class CreateRequestBuilder : ICreateRequestBuilder
    {
        public ICreateGroupInitBuilder Group => new CreateGroupInitBuilder();
    }

}