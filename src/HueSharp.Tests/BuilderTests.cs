using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using HueSharp.Builder;
using HueSharp.Messages;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class BuilderTests : TestBase
    {
        public BuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void BuildGetAllLightsRequest()
        {
            var request = HueRequestBuilder.Select.Lights().Build();
            Assert.Equal("lights", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void BuildGetLightRequest()
        {
            var request = HueRequestBuilder.Select.Light(7).Build();

            Assert.Equal("lights/7", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void BuildTurnOnOffRequest()
        {
            var builder = HueRequestBuilder.Modify.Light(7).TurnOn();

            var request = builder.Build();

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.True(statusRequest.Status.IsOn);

            builder.TurnOff();
            request = builder.Build();

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.False(statusRequest.Status.HasUnsavedChanges);
            Assert.False(statusRequest.Status.ShouldSerializeIsOn());
            Assert.False(statusRequest.Status.IsOn);
        }

    }
}
