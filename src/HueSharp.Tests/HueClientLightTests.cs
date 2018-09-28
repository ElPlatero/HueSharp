using HueSharp.Messages;
using HueSharp.Messages.Lights;
using System;
using System.Linq;
using System.Threading.Tasks;
using HueSharp.Enums;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientLightTests : TestBase
    {
        public HueClientLightTests(ITestOutputHelper outputHelper) 
            : base(outputHelper) { }


        [ExplicitFact]
        public async Task GetLightStateTest()
        {
            var request = new GetLightStateRequest {LightId = 7};

            var response = await _client.GetResponseAsync(request);

            Assert.True(response != null);
            Assert.True(response.GetType() != typeof(ErrorResponse));

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetLightStateTest()
        {
            IHueRequest request = new GetLightStateRequest(7);
            var response = await _client.GetResponseAsync(request) as GetLightStateResponse;
            Assert.NotNull(response);

            request = new SetLightStateRequest(7);
            ((SetLightStateRequest) request).Status.IsOn = !response.Status.IsOn;
            if (!response.Status.IsOn)
            {
                ((SetLightStateRequest)request).Status.Hue = new Random().Next(0, ushort.MaxValue);
                ((SetLightStateRequest) request).Status.Effect = LightEffect.ColorLoop;
            }

            _client.GetResponseAsync(request).Wait();

            Assert.True(!((SetLightStateRequest)request).Status.HasUnsavedChanges);
         }

        [ExplicitFact]
        public async Task GetAllLightsTest()
        {
            IHueRequest request = new GetAllLightsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetAllLightsResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task CreateErrorObjectTest()
        {
            IHueRequest initialRequest = new GetAllLightsRequest();
            var initialResponse = (GetAllLightsResponse)await _client.GetResponseAsync(initialRequest);
            var id = initialResponse.First(p => !p.Status.IsOn).Id;

            var request = new SetLightStateRequest(id) {Status = {BrightnessIncrement = 100}};

            await Assert.ThrowsAsync<HueResponseException>(() => _client.GetResponseAsync(request));
        }

        [ExplicitFact]
        public async Task GetNewLightsTest()
        {
            IHueRequest request = new GetNewLightsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetNewLightsResponse);
            Assert.Empty((GetNewLightsResponse)response);
        }

        [ExplicitFact]
        public async Task SearchNewLightsTest()
        {
            IHueRequest request = new SearchNewLightsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);

            request = new GetNewLightsRequest();
            response = await _client.GetResponseAsync(request);
            Assert.True(response is GetNewLightsResponse);
            Assert.True(((GetNewLightsResponse)response).LastScan == DateTime.MaxValue);
        }

        [ExplicitFact]
        public async Task SetLightAttributesTest()
        {
            IHueRequest request = new SetLightAttributesRequest(7, "Testname");

            var response = await _client.GetResponseAsync(request);

            Assert.True(response != null);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task DeleteLightTest()
        {
            var request = new DeleteLightRequest(7);
            await Assert.ThrowsAsync<ArgumentException>(() => _client.GetResponseAsync(request));
        }

    }
}
