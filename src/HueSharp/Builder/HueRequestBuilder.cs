namespace HueSharp.Builder
{
    public static class HueRequestBuilder
    {
        public static GetRequestBuilder Select => new GetRequestBuilder();
        public static SetRequestBuilder Modify => new SetRequestBuilder();
    }
}