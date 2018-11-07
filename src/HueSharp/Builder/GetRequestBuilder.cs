namespace HueSharp.Builder
{
    class GetRequestBuilder : IGetRequestBuilder
    {
        public IBuilder Lights => new GetAllLightsRequestBuilder();
        public IBuilder Light(int lightId) => new GetLightStateRequestBuilder(lightId);
        public IBuilder Groups => new GetAllGroupsRequestBuilder();
        public IBuilder Group(int groupId) => new GetGroupStateRequestBuilder(groupId);
    }
}