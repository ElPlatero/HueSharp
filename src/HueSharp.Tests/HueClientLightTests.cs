using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Net;
using System;
using System.Linq;
using Xunit;

namespace HueSharp.Tests
{
    internal class HueClientLightTests
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.100.14";

        private HueClient _client;

        public void Setup()
        {
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            _client.Log += (s, e) => Console.WriteLine(e);
        }

        public void GetLightStateTest()
        {
            var request = new GetLightStateRequest();
            request.LightId = 7;

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response != null);
            Assert.True(response.GetType() != typeof(ErrorResponse));
        }

        public void SetLightStateTest()
        {
            IHueRequest request = new GetLightStateRequest(7);
            GetLightStateResponse response = _client.GetResponse(request) as GetLightStateResponse;

            request = new SetLightStateRequest(7);

            if (!response.Status.IsOn) ((SetLightStateRequest)request).Status.IsOn = true;
            ((SetLightStateRequest)request).Status.Hue = new Random().Next(0, ushort.MaxValue);

            _client.GetResponse(request);
            Assert.True(!((SetLightStateRequest)request).Status.HasUnsavedChanges);
         }

        public void GetAllLightsTest()
        {
            var request = new GetAllLightsRequest();
            var response = _client.GetResponse(request);
            ((GetAllLightsResponse)response).ForEach(p => Console.WriteLine("{0} - {1} ({2})", p.Id, p.Name, p.Status.IsOn ? "an" : "aus"));
        }

        public void CreateErrorObjectTest()
        {
            var initialRequest = new GetAllLightsRequest();
            var initialResponse = (GetAllLightsResponse)_client.GetResponse(initialRequest);

            var id = initialResponse.First(p => !p.Status.IsOn).Id;

            var request = new SetLightStateRequest(id);
            request.Status.BrightnessIncrement = 100;

            IHueResponse result = null;
            Assert.Throws<HueResponseException>(() => result = _client.GetResponse(request));
        }

        public void GetNewLightsTest()
        {
            var request = new GetNewLightsRequest();

            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.False(response is ErrorResponse);
        }

        public void SearchNewLightsTest()
        {
            IHueRequest request = new SearchNewLightsRequest();

            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);

            request = new GetNewLightsRequest();
            response = _client.GetResponse(request);
            Assert.True(response is GetNewLightsResponse);
            Assert.True(((GetNewLightsResponse)response).LastScan == DateTime.MaxValue);
        }

        public void SetLightAttributesTest()
        {
            var request = new SetLightAttributesRequest(7, "Testname");

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response != null);
            Assert.True(response is SuccessResponse);
        }

        public void DeleteLightTest()
        {
            var request = new DeleteLightRequest(7);
            IHueResponse response = null;

            Assert.Throws<ArgumentException>(() => response = _client.GetResponse(request));
        }
    }
}
