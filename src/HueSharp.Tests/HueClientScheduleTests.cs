using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Schedules;
using System;
using System.Threading.Tasks;
using HueSharp.Builder;
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
            _tmpScheduleId = CreateTemporarySchedule().Result;
        }

        public void Dispose()
        {
            DeleteTemporarySchedule(_tmpScheduleId).Wait();
        }

        [ExplicitFact]
        public async Task CreateScheduleTest()
        {

            var commandState = HueRequestBuilder.Modify.Light(7).Status.TurnOn().Build();
            var newSchedule = new GetScheduleResponse
            {
                AutoDelete = true,
                Name = "new Timer",
                Description = "testing that scheduling",
                Timing = ScheduleTiming.CreateNew(ScheduleTimingTypes.Alarm),
                Command = new Command(commandState),
                Status = ScheduleStatus.Enabled
            };

            newSchedule.Timing.BaseDate = DateTime.Now.AddDays(1);

            IHueRequest request = new CreateScheduleRequest {NewSchedule = newSchedule};

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
            Assert.True(newSchedule.Id > 0);

            await DeleteTemporarySchedule(newSchedule.Id);
        }

        [ExplicitFact]
        public async Task GetAllSchedulesTest()
        {
            IHueRequest request = new GetAllSchedulesRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetAllSchedulesResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task GetScheduleTest()
        {
            IHueRequest request = new GetScheduleRequest(_tmpScheduleId);

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetScheduleResponse, "response is GetScheduleResponse");
            OnLog(response);
        }

        [ExplicitFact]
        public async Task SetScheduleTest()
        {
            const string expectedDescription = "new description";

            var mySchedule = await _client.GetResponseAsync(new GetScheduleRequest(_tmpScheduleId)) as GetScheduleResponse;
            Assert.NotNull(mySchedule);

            var request = new SetScheduleRequest(mySchedule) {Schedule = {Description = expectedDescription}};

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            Assert.Equal(((SuccessResponse)response)["description"].ToString(), expectedDescription);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task DeleteScheduleTest()
        {
            IHueRequest request = new DeleteScheduleRequest(await CreateTemporarySchedule());

            var response = await _client.GetResponseAsync(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        private async Task<int> CreateTemporarySchedule()
        {
            var request = new CreateScheduleRequest();
            var commandState = HueRequestBuilder.Modify.Light(7).Status.TurnOn().Build();
            var newSchedule = new GetScheduleResponse
            {
                AutoDelete = true,
                Name = "temporary schedule",
                Description = "temporary schedule description",
                Timing = ScheduleTiming.CreateNew(ScheduleTimingTypes.Alarm),
                Command = new Command(commandState),
                Status = ScheduleStatus.Enabled
            };

            newSchedule.Timing.BaseDate = DateTime.Now.AddDays(1);
            request.NewSchedule = newSchedule;

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse, "response is SuccessResponse");
            OnLog(response);
            Assert.True(newSchedule.Id > 0, "new ID set");
            return newSchedule.Id;
        }

        private async Task DeleteTemporarySchedule(int id)
        {
            await _client.GetResponseAsync(new DeleteScheduleRequest(id));
        }

    }
}
