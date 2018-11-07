using HueSharp.Messages;
using HueSharp.Messages.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueSharp.Builder;
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
            var request = HueRequestBuilder.Select.Light(7).Build();

            var response = await _client.GetResponseAsync(request);

            Assert.True(response != null);
            Assert.True(response.GetType() != typeof(ErrorResponse));

            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetLightStateTest()
        {
            const int LIGHT_ID = 4;

            IHueRequest request = HueRequestBuilder.Select.Light(LIGHT_ID).Build();
            var response = await _client.GetResponseAsync(request);
            Assert.True(response is IHueStatusMessage);

            var builder = HueRequestBuilder.Modify.Light(LIGHT_ID);
            if (((IHueStatusMessage)response).Status.IsOn) builder.Status.During(TimeSpan.FromSeconds(10)).TurnOff();
            else
            {
                builder.Status
                    .TurnOn()
                    .Color(46920, 254, 254)
                    .ColorLoop();
            }

            request = builder.Status.Build();

            response = await _client.GetResponseAsync(request);

            Assert.True(response is IHueStatusMessage);
         }

        [ExplicitFact]
        public async Task GetAllLightsTest()
        {
            IHueRequest request = HueRequestBuilder.Select.Lights.Build();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is ICollection<Light>);

            OnLog(response);
        }

        [ExplicitFact]
        public async Task CreateErrorObjectTest()
        {
            IHueRequest initialRequest = HueRequestBuilder.Select.Lights.Build();
            var initialResponse = await _client.GetResponseAsync(initialRequest);

            var lights = initialResponse as ICollection<Light>;
            Assert.NotNull(lights);

            var id = lights.First(p => !p.Status.IsOn).Id;

            var request = HueRequestBuilder.Modify.Light(id).Status.Increase.Brightness.By(100).Build();

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
