using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Schedules;
using HueSharp.Net;
using System;
using Xunit;

namespace HueSharp.Tests
{
    public class HueClientScheduleTests
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.100.14";

        private HueClient _client;
        private int _tmpScheduleId;

        public void Setup()
        {
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            _tmpScheduleId = CreateTemporarySchedule();

            _client.Log += ClientOnLog;

        }

        public void TearDown()
        {
            _client.Log -= ClientOnLog;
            DeleteTemporarySchedule(_tmpScheduleId);
        }

        public void CreateScheduleTest()
        {
            var request = new CreateScheduleRequest();

            var commandState = new SetLightStateRequest(7) { Status = new SetLightState { IsOn = true, TransitionTime = TimeSpan.FromSeconds(1) } };

            var newSchedule = new GetScheduleResponse
            {
                AutoDelete = true,
                Name = "new Timer",
                Description = "testing that scheduling",
                Timing = ScheduleTiming.CreateNew(ScheduleTimingTypes.Alarm),
                Command = new Command(commandState, p => ((SetLightStateRequest)p).Status),
                Status = ScheduleStatus.Enabled
            };

            newSchedule.Timing.BaseDate = DateTime.Now.AddDays(1);

            request.NewSchedule = newSchedule;

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            Assert.True(newSchedule.Id > 0);
            DeleteTemporarySchedule(newSchedule.Id);
        }

        public void GetAllSchedulesTest()
        {
            var request = new GetAllSchedulesRequest();
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetAllSchedulesResponse);

            ((GetAllSchedulesResponse)response).ForEach(p => Console.WriteLine($"{p.Id} - \"{p.Name}\" ({p.Description}): {p.Timing.ToJson()}"));
        }

        public void GetScheduleTest()
        {
            var request = new GetScheduleRequest(_tmpScheduleId);

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetScheduleResponse, "response is GetScheduleResponse");
            var p = (GetScheduleResponse)response;
            Console.WriteLine($"{p.Id} - \"{p.Name}\" ({p.Description}): {p.Timing.ToJson()}");
        }

        public void SetScheduleTest()
        {
            const string expectedDescription = "new description";

            var mySchedule = _client.GetResponse(new GetScheduleRequest(_tmpScheduleId)) as GetScheduleResponse;

            Assert.NotNull(mySchedule);

            var request = new SetScheduleRequest(mySchedule);
            request.Schedule.Description = expectedDescription;

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            Assert.Equal(((SuccessResponse)response)["description"].ToString(), expectedDescription);
        }
        public void DeleteScheduleTest()
        {
            var request = new DeleteScheduleRequest(CreateTemporarySchedule());
            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }

        private int CreateTemporarySchedule()
        {
            var request = new CreateScheduleRequest();
            var commandState = new SetLightStateRequest(7) { Status = new SetLightState { IsOn = true, TransitionTime = TimeSpan.FromSeconds(1) } };
            var newSchedule = new GetScheduleResponse
            {
                AutoDelete = true,
                Name = "temporary schedule",
                Description = "temporary schedule description",
                Timing = ScheduleTiming.CreateNew(ScheduleTimingTypes.Alarm),
                Command = new Command(commandState, p => ((SetLightStateRequest)p).Status),
                Status = ScheduleStatus.Enabled
            };

            newSchedule.Timing.BaseDate = DateTime.Now.AddDays(1);
            request.NewSchedule = newSchedule;

            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            Assert.True(newSchedule.Id > 0, "new ID set");
            return newSchedule.Id;
        }

        private void DeleteTemporarySchedule(int id)
        {
            _client.GetResponse(new DeleteScheduleRequest(id));
        }

        private void ClientOnLog(object sender, string e) => Console.WriteLine(e);
    }
}
