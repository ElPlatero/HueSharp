using HueSharp.Messages;
using HueSharp.Messages.Groups;
using System;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientGroupTests : TestBase
    {
        public HueClientGroupTests(ITestOutputHelper outputHelper) 
            : base(outputHelper) { }


        [ExplicitFact]
        public void GetAllGroupsTest()
        {
            var request = new GetAllGroupsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetAllGroupsResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public void GetGroupTest()
        {
            var request = new GetGroupRequest(3);

            var response = _client.GetResponse(request);
            Assert.True(response is GetGroupResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public void SetGroupTest()
        {
            IHueRequest request = new GetGroupRequest(1);
            IHueResponse response = _client.GetResponse(request);

            OnLog(response);

            request = new SetGroupRequest((GetGroupResponse)response);
            ((SetGroupRequest)request).NewName = "Testname";

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public void SetGroupStateTest()
        {
            IHueRequest request = new SetGroupStateRequest(3, new SetGroupState { IsOn = false });

            IHueResponse response = _client.GetResponse(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void CreateAndDeleteGroupTest()
        {
            IHueRequest request = new CreateGroupRequest("tmp testgroup", 6, 7);

            IHueResponse response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            var newGroupId = Convert.ToInt32(((SuccessResponse)response)["id"]);

            request = new DeleteGroupRequest(newGroupId);

            Assert.Throws<ArgumentException>(() => response = _client.GetResponse(request));
            ((DeleteGroupRequest)request).Acknowledge();

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            OnLog(response);
        }
    }
}
