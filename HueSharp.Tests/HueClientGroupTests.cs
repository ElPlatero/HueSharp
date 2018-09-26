using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Groups;
using HueSharp.Net;
using System;
using Xunit;

namespace HueSharp.Tests
{
    class HueClientGroupTests
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.100.14";

        private HueClient _client;

        public void Setup()
        {
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            _client.Log += (s, e) => Console.WriteLine(e);
        }

        public void GetAllGroupsTest()
        {
            var request = new GetAllGroupsRequest();
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetAllGroupsResponse);
            ((GetAllGroupsResponse)response).ForEach(p => Console.WriteLine("{0} - \"{1}\" ({2}): {3}"
                , p.Id
                , p.Name
                , string.Join(",", p.LightIds)
                , p.State.AllOn ? "alle an" : p.State.AnyOn ? "einige an" : "alle aus"));
        }

        public void GetGroupTest()
        {
            var request = new GetGroupRequest(3);
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetGroupResponse);

            var p = (GetGroupResponse)response;

            Console.WriteLine("{0} - \"{1}\" ({2}): {3}"
                , p.Id
                , p.Name
                , string.Join(",", p.LightIds)
                , p.State.AllOn ? "alle an" : p.State.AnyOn ? "einige an" : "alle aus");
        }

        public void SetGroupTest()
        {
            IHueRequest request = new GetGroupRequest(1);
            IHueResponse response = _client.GetResponse(request);

            request = new SetGroupRequest((GetGroupResponse)response);
            ((SetGroupRequest)request).NewName = "Testname";

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }

        public void SetGroupStateTest()
        {
            var request = new SetGroupStateRequest(3, new SetGroupState { IsOn = false });

            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }

        public void CreateAndDeleteGroupTest()
        {
            IHueRequest request = new CreateGroupRequest("tmp testgroup", 6, 7);
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            var newGroupId = Convert.ToInt32(((SuccessResponse)response)["id"]);

            request = new DeleteGroupRequest(newGroupId);
            Assert.Throws<ArgumentException>(() => response = _client.GetResponse(request));
            ((DeleteGroupRequest)request).Acknowledge();
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }
    }
}
