using HueSharp.Messages;
using HueSharp.Messages.Groups;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientGroupTests : TestBase
    {
        public HueClientGroupTests(ITestOutputHelper outputHelper) 
            : base(outputHelper) { }


        [ExplicitFact]
        public async Task GetAllGroupsTest()
        {
            var request = new GetAllGroupsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetAllGroupsResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task GetGroupTest()
        {
            var request = new GetGroupRequest(3);

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetGroupResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetGroupTest()
        {
            IHueRequest request = new GetGroupRequest(1);
            IHueResponse response = await _client.GetResponseAsync(request);

            OnLog(response);

            request = new SetGroupRequest((GetGroupResponse)response);
            ((SetGroupRequest)request).NewName = "Testname";

            response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetGroupStateTest()
        {
            IHueRequest request = new SetGroupStateRequest(3, new SetGroupState { IsOn = false });

            IHueResponse response = await _client.GetResponseAsync(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task CreateAndDeleteGroupTest()
        {
            IHueRequest request = new CreateGroupRequest("tmp testgroup", 6, 7);

            IHueResponse response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);

            var newGroupId = Convert.ToInt32(((SuccessResponse)response)["id"]);

            request = new DeleteGroupRequest(newGroupId);

            await Assert.ThrowsAsync<ArgumentException>(async () => response = await _client.GetResponseAsync(request));
            ((DeleteGroupRequest)request).Acknowledge();

            response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);

            OnLog(response);
        }
    }
}
