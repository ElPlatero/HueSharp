using System.Net.Http;
using HueSharp.Converters;
using Newtonsoft.Json;

namespace HueSharp.Messages.Schedules
{
    public class DeleteScheduleRequest : HueRequestBase
    {
        public int ScheduleId { get; }

        public DeleteScheduleRequest() : this(-1) { }
        public DeleteScheduleRequest(int scheduleId) : base("schedules", HttpMethod.Delete)
        {
            ScheduleId = scheduleId;
        }

        public override string Address => $"{base.Address}/{ScheduleId}";

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
