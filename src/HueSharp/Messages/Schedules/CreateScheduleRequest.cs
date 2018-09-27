using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Schedules
{
    public class CreateScheduleRequest : HueRequestBase, IUploadable, IContainsCommand
    {
        public GetScheduleResponse NewSchedule { get; set; }

        public CreateScheduleRequest() : base("schedules", HttpMethod.Post) { }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(NewSchedule);
        }

        public Command Command => NewSchedule?.Command;

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            NewSchedule.Id = Convert.ToInt32(result["id"]);
            return result;
        }
    }
}
