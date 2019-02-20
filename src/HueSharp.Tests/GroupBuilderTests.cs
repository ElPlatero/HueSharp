using System;
using System.Drawing;
using System.Net.Http;
using HueSharp.Builder;
using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class GroupBuilderTests : TestBase
    {
        public GroupBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void BuildGetAllGroupsRequestTest()
        {
            var request = HueRequestBuilder.Select.Groups.Build();

            Assert.Equal("groups", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void BuildGetGroupRequestTest()
        {
            var request = HueRequestBuilder.Select.Group(1).Build();

            Assert.Equal("groups/1", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void ModifyNameTest()
        {
            const string NAME = "my Name";

            var request = HueRequestBuilder.Modify.Group(1).Attributes.UseTheseLights(1, 2, 3, 4, 5).Name(NAME).Build();

            Assert.Equal("groups/1", request.Address);
            AssertRequestBody(request, ("name", NAME));
        }

        [Fact]
        public void ModifyClassTest()
        {
            var request = HueRequestBuilder.Modify.Group(1).Attributes.UseTheseLights(1).Class(RoomClass.Recreation).Build();

            Assert.Equal("groups/1", request.Address);
            AssertRequestBody(request, ("class", "Recreation"));
        }

        [Fact]
        public void AlertTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Alert("<fzwehf");
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.False(statusRequest.Status.HasUnsavedChanges);
            Assert.False(statusRequest.Status.ShouldSerializeAlert());

            builder.Alert(LightAlert.Cycle);
            request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeAlert());
            Assert.Equal(LightAlert.Cycle, statusRequest.Status.Alert);
        }

        [Fact]
        public void BrightnessTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Brightness(254);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeBrightness());
            Assert.Equal(254, statusRequest.Status.Brightness);

            builder.Brightness(100);
            request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeBrightness());
            Assert.Equal(100, statusRequest.Status.Brightness);
        }

        [Fact]
        public void TurnOnOffRequestTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.TurnOn();

            var request = builder.Build();

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.True(statusRequest.Status.IsOn);

            builder.TurnOff(); //TurnOn().TurnOff() => these two void each other, so nothing is expected to happen.
            request = builder.Build();

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.False(statusRequest.Status.HasUnsavedChanges);
            Assert.False(statusRequest.Status.ShouldSerializeIsOn());
            Assert.False(statusRequest.Status.IsOn);

            builder.TurnOff();  //TurnOn().TurnOff().TurnOff() //after voiding and explicitely turning off, we expect a turnoff to be sent.
            request = builder.Build();

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.False(statusRequest.Status.IsOn);

            builder.TurnOff();  //an additional TurnOff() should have no further effect
            request = builder.Build();

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.False(statusRequest.Status.IsOn);

            builder.TurnOn();
            request = builder.Build();  //this should negate the TurnOff(), resulting in a request that does not change the status.

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.False(statusRequest.Status.HasUnsavedChanges);
            Assert.False(statusRequest.Status.ShouldSerializeIsOn());
            Assert.False(statusRequest.Status.IsOn);

            builder.TurnOn();
            request = builder.Build();  //a last TurnOn().

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.True(statusRequest.Status.IsOn);

            builder.TurnOn();
            request = builder.Build();  //an additional TurnOn() should have no further effect.

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeIsOn());
            Assert.True(statusRequest.Status.IsOn);

            //all cases covered.
        }
        [Fact]
        public void CieLocationTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.CieLocation(0.3, 0.7);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeCoordinates());
            Assert.Equal(0.3, statusRequest.Status.Coordinates[0]);
            Assert.Equal(0.7, statusRequest.Status.Coordinates[1]);
        }

        [Fact]
        public void HueTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Hue(49960);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeHue());
            Assert.Equal(49960, statusRequest.Status.Hue);

            builder.Hue(20000);
            request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeHue());
            Assert.Equal(20000, statusRequest.Status.Hue);
        }

        [Fact]
        public void SaturationTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Saturation(254);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeSaturation());
            Assert.Equal(254, statusRequest.Status.Saturation);

            builder.Saturation(100);
            request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeSaturation());
            Assert.Equal(100, statusRequest.Status.Saturation);
        }

        [Fact]
        public void ColorHsbTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Color(40000, 100, 128);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeHue());
            Assert.True(statusRequest.Status.ShouldSerializeSaturation());
            Assert.True(statusRequest.Status.ShouldSerializeBrightness());
            Assert.Equal(40000, statusRequest.Status.Hue);
            Assert.Equal(100, statusRequest.Status.Saturation);
            Assert.Equal(128, statusRequest.Status.Brightness);
        }

        [Fact]
        public void ColorTemperatureTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.ColorTemperature(4500);
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeColorTemperature());
            Assert.Equal(4500, statusRequest.Status.ColorTemperature);
        }

        [Fact]
        public void TransitionTimeTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.During(TimeSpan.FromSeconds(10));
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.False(statusRequest.Status.HasUnsavedChanges);
            Assert.False(statusRequest.Status.ShouldSerializeTransitionTimeAsNumber());

            builder.TurnOn();
            request = builder.Build();

            statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeTransitionTimeAsNumber());
            Assert.Equal(10, statusRequest.Status.TransitionTime.TotalSeconds);
        }

        [Fact]
        public void ColorLoopTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.ColorLoop();
            var request = builder.Build();

            Assert.Equal("groups/1/action", request.Address);

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeEffect());
            Assert.Equal(LightEffect.ColorLoop, statusRequest.Status.Effect);
        }

        [Fact]
        public void SetRgbColorTest()
        {
            var builder = HueRequestBuilder.Modify.Group(1).Status.Color(Color.Red);
            var request = builder.Build();

            var statusRequest = request as IHueStatusMessage;
            Assert.NotNull(statusRequest);
            Assert.True(statusRequest.Status.HasUnsavedChanges);
            Assert.True(statusRequest.Status.ShouldSerializeCoordinates());
            Assert.NotEmpty(statusRequest.Status.Coordinates);
            Assert.Collection(statusRequest.Status.Coordinates,
                p => Assert.True(p >= 0 && p <= 1),
                p => Assert.True(p >= 0 && p <= 1)
            );
        }

    }
}
