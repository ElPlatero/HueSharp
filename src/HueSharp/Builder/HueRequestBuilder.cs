namespace HueSharp.Builder
{
    public static class HueRequestBuilder
    {
        public static IGetRequestBuilder Select => new GetRequestBuilder();
        public static ISetRequestBuilder Modify => new SetRequestBuilder();
        public static ICreateRequestBuilder Create => new CreateRequestBuilder();
    }
}