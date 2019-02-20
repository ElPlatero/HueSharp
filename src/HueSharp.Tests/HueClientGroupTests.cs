using HueSharp.Messages;
using HueSharp.Messages.Groups;
using System;
using System.Threading.Tasks;
using HueSharp.Builder;
using HueSharp.Enums;
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
            var request = HueRequestBuilder.Select.Groups.Build();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetAllGroupsResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task GetGroupTest()
        {
            var request = HueRequestBuilder.Select.Group(3).Build();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetGroupResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetGroupTest()
        {
            IHueRequest request = HueRequestBuilder.Select.Group(1).Build();
            IHueResponse response = await _client.GetResponseAsync(request);

            OnLog(response);

            request = HueRequestBuilder.Modify.Group(response).Attributes.UseLightsInResponse().Name("Testname").Build();

            response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetGroupStateTest()
        {
            IHueRequest request = HueRequestBuilder.Modify.Group(3).Status.TurnOff().Build();

            IHueResponse response = await _client.GetResponseAsync(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task CreateAndDeleteGroupTest()
        {
            IHueRequest request = HueRequestBuilder.Create.Group.New(6, 7).Name("tmp testgroup").Class(GroupType.Room).Build();

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
