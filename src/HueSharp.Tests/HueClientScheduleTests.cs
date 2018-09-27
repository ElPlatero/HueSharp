using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Schedules;
using System;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientScheduleTests : TestBase, IDisposable
    {
        private readonly int _tmpScheduleId;

        public HueClientScheduleTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            _tmpScheduleId = CreateTemporarySchedule();
        }

        public void Dispose()
        {
            DeleteTemporarySchedule(_tmpScheduleId);
        }

        [ExplicitFact]
        public void CreateScheduleTest()
        {

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

            IHueRequest request = new CreateScheduleRequest {NewSchedule = newSchedule};

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
            Assert.True(newSchedule.Id > 0);

            DeleteTemporarySchedule(newSchedule.Id);
        }

        [ExplicitFact]
        public void GetAllSchedulesTest()
        {
            IHueRequest request = new GetAllSchedulesRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetAllSchedulesResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void GetScheduleTest()
        {
            IHueRequest request = new GetScheduleRequest(_tmpScheduleId);

            var response = _client.GetResponse(request);
            Assert.True(response is GetScheduleResponse, "response is GetScheduleResponse");
            OnLog(response);
        }

        [ExplicitFact]
        public void SetScheduleTest()
        {
            const string expectedDescription = "new description";

            var mySchedule = _client.GetResponse(new GetScheduleRequest(_tmpScheduleId)) as GetScheduleResponse;
            Assert.NotNull(mySchedule);

            var request = new SetScheduleRequest(mySchedule) {Schedule = {Description = expectedDescription}};

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            Assert.Equal(((SuccessResponse)response)["description"].ToString(), expectedDescription);
            OnLog(response);
        }

        [ExplicitFact]
        public void DeleteScheduleTest()
        {
            IHueRequest request = new DeleteScheduleRequest(CreateTemporarySchedule());

            var response = _client.GetResponse(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
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

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            OnLog(response);

            Assert.True(newSchedule.Id > 0, "new ID set");
            return newSchedule.Id;
        }

        private void DeleteTemporarySchedule(int id)
        {
            _client.GetResponse(new DeleteScheduleRequest(id));
        }

    }
}
