using HueSharp.Messages;
using HueSharp.Messages.Lights;
using System;
using System.Linq;
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
        public void GetLightStateTest()
        {
            var request = new GetLightStateRequest {LightId = 7};

            var response = _client.GetResponse(request);

            Assert.True(response != null);
            Assert.True(response.GetType() != typeof(ErrorResponse));

            OnLog(response);
        }

        [ExplicitFact]
        public void SetLightStateTest()
        {
            IHueRequest request = new GetLightStateRequest(7);
            var response = _client.GetResponse(request) as GetLightStateResponse;
            Assert.NotNull(response);

            request = new SetLightStateRequest(7);
            ((SetLightStateRequest) request).Status.IsOn = !response.Status.IsOn;
            if (!response.Status.IsOn)
            {
                ((SetLightStateRequest)request).Status.Hue = new Random().Next(0, ushort.MaxValue);
                ((SetLightStateRequest) request).Status.Effect = LightEffect.ColorLoop;
            }

            _client.GetResponse(request);

            Assert.True(!((SetLightStateRequest)request).Status.HasUnsavedChanges);
         }

        [ExplicitFact]
        public void GetAllLightsTest()
        {
            IHueRequest request = new GetAllLightsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetAllLightsResponse);

            OnLog(response);
        }

        [ExplicitFact]
        public void CreateErrorObjectTest()
        {
            IHueRequest initialRequest = new GetAllLightsRequest();
            var initialResponse = (GetAllLightsResponse)_client.GetResponse(initialRequest);
            var id = initialResponse.First(p => !p.Status.IsOn).Id;

            var request = new SetLightStateRequest(id);
            request.Status.BrightnessIncrement = 100;

            IHueResponse result = null;
            Assert.Throws<HueResponseException>(() => result = _client.GetResponse(request));
        }

        [ExplicitFact]
        public void GetNewLightsTest()
        {
            IHueRequest request = new GetNewLightsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetNewLightsResponse);
            Assert.Empty((GetNewLightsResponse)response);
        }

        [ExplicitFact]
        public void SearchNewLightsTest()
        {
            IHueRequest request = new SearchNewLightsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            request = new GetNewLightsRequest();
            response = _client.GetResponse(request);
            Assert.True(response is GetNewLightsResponse);
            Assert.True(((GetNewLightsResponse)response).LastScan == DateTime.MaxValue);
        }

        [ExplicitFact]
        public void SetLightAttributesTest()
        {
            IHueRequest request = new SetLightAttributesRequest(7, "Testname");

            var response = _client.GetResponse(request);

            Assert.True(response != null);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void DeleteLightTest()
        {
            var request = new DeleteLightRequest(7);

            IHueResponse response = null;
            Assert.Throws<ArgumentException>(() => response = _client.GetResponse(request));
            Assert.Null(response);
        }

    }
}
