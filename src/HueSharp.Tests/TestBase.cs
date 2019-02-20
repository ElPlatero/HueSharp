using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HueSharp.Messages;
using HueSharp.Messages.Groups;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Scenes;
using HueSharp.Messages.Schedules;
using HueSharp.Messages.Sensors;
using HueSharp.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public abstract class TestBase
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.178.46/";

        protected readonly HueClient _client;
        private readonly ILogger _logger;

        protected TestBase(ITestOutputHelper outputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(outputHelper));
            _client = new HueClient(loggerFactory, DEV_USER, DEV_ADDRESS);
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        protected void OnLog(string message) => _logger.LogDebug(message);

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
                case IHueStatusMessage statusMessage:
                    OnLog(ToString(statusMessage));
                    break;
                case ICollection<Light> lightCollection:
                    OnLog(ToString(lightCollection));
                    break;
                case GetAllScenesResponse getAllScenesResponse:
                    OnLog(ToString(getAllScenesResponse));
                    break;
                case GetAllSchedulesResponse getAllSchedulesResponse:
                    OnLog(ToString(getAllSchedulesResponse));
                    break;
                case GetAllSensorsResponse getAllSensorsResponse:
                    OnLog(ToString(getAllSensorsResponse));
                    break;
                case SuccessResponse successResponse:
                    OnLog(ToString(successResponse));
                    break;
            }
        }

        protected void AssertRequestBody(IHueRequest request, params (string propertyName, string value)[] checks)
        {
            var uploadable = request as IUploadable;
            Assert.NotNull(uploadable);

            var jObject = JObject.Parse(uploadable.GetRequestBody());

            foreach (var check in checks)
            {
                Assert.NotNull(jObject[check.propertyName]);
                Assert.Equal(check.value, jObject[check.propertyName].Value<string>());
            }
        }

        #region static
        private static string ToString(GetAllGroupsResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }

        private static string ToString(ICollection<Light> p)
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

        private static string ToString(GetScheduleResponse p)
        {
            return $@"{p.Id} - ""{p.Name}"" ({p.Description}): {p.Timing.ToJson()}";
        }

        private static string ToString(GetAllSchedulesResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }

        private static string ToString(SensorBase p)
        {
            return $@"{p.Name}: {p.Type}, {p.ManufacturerName}";
        }

        private static string ToString(GetAllSensorsResponse p)
        {
            return string.Join(Environment.NewLine, p.Select(ToString));
        }

        private static string ToString(IHueStatusMessage p)
            => $"{(p.Status.IsOn ? "on" : "off")}";

        private static string ToString(SuccessResponse p)
            => string.Join(", ", p.Select(q => $"{q.Key}: {q.Value}"));
        #endregion

    }
}