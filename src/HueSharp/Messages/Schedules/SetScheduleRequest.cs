using System.Net.Http;
using HueSharp.Converters;
using Newtonsoft.Json;

namespace HueSharp.Messages.Schedules
{
    public class SetScheduleRequest : HueRequestBase, IUploadable
    {
        public GetScheduleResponse Schedule { get; set; }

        public SetScheduleRequest(GetScheduleResponse schedule = null) : base("schedules", HttpMethod.Put)
        {
            Schedule = schedule;
        }

        public override string Address => $"{base.Address}/{Schedule.Id}";

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(Schedule);
        }
    }
}
