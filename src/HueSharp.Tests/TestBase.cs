using System;
using System.Globalization;
using System.Linq;
using HueSharp.Messages;
using HueSharp.Messages.Groups;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Scenes;
using HueSharp.Net;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public abstract class TestBase
    {
        private readonly ITestOutputHelper _outputHelper;
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.178.46/";

        protected HueClient _client;

        protected TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            _client.Log += OnLog;
        }

        protected void OnLog(string message) => OnLog(this, message);

        protected void OnLog(IHueResponse response)
        {
            switch (response)
            {
                case GetGroupResponse getGroupResponse:
                    OnLog(ToString(getGroupResponse));
                    break;
                case GetAllGroupsResponse getAllGroupsResponse:
                    OnLog(ToString(getAllGroupsResponse));
                    break;
                case GetLightStateResponse getLightStateResponse:
                    OnLog(ToString(getLightStateResponse));
                    break;
                case GetAllLightsResponse getAllLightsResponse:
                    OnLog(ToString(getAllLightsResponse));
                    break;
                case GetAllScenesResponse getAllScenesResponse:
                    OnLog(ToString(getAllScenesResponse));
                    break;
                case SuccessResponse successResponse:
                    OnLog(ToString(successResponse));
                    break;
            }
        }


        private void OnLog(object sender, string message)
            => _outputHelper.WriteLine($"{sender.GetType().Name}: {message}");

        private static string ToString(GetAllGroupsResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }

        private static string ToString(GetAllLightsResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }

        private static string ToString(GetGroupResponse p)
        {
            return $@"{p.Id} - ""{p.Name}"" ({string.Join(",", p.LightIds)}): {(p.State.AllOn ? "all on" : p.State.AnyOn ? "some on" : "all off")}";
        }

        private static string ToString(Light p)
        {
            return $@"{p.Type} ""{p.Name}"": {(p.Status.IsOn ? "on":"off")}, {(p.Status.IsReachable ? "available" : "not available")}";
        }

        private static string ToString(GetSceneResponse p)
        {
            return $@"{p.Name}: {string.Join(", ", p.LightIds.Select(q => q.ToString(CultureInfo.InvariantCulture)))}";
        }

        private static string ToString(GetAllScenesResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }



        private static string ToString(GetLightStateResponse p)
            => $"{p.Name}: {(p.Status.IsOn ? "on" : "off")}";

        private static string ToString(SuccessResponse p)
            => string.Join(", ", p.Select(q => $"{q.Key}: {q.Value}"));


    }
}