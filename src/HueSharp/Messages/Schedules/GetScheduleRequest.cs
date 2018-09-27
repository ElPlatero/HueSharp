using System.Net.Http;
using Newtonsoft.Json;

namespace HueSharp.Messages.Schedules
{
    public class GetScheduleRequest : HueRequestBase
    {
        public int ScheduleId { get; }

        public GetScheduleRequest() : this(-1) { }
        public GetScheduleRequest(int scheduleId) : base("schedules", HttpMethod.Get) { ScheduleId = scheduleId; }

        public override string Address => $"{base.Address}/{ScheduleId}";

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<GetScheduleResponse>(json);
            result.Id = ScheduleId;

            return result;
        }
    }
}
