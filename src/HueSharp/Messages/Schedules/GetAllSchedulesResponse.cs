using System.Collections.Generic;
using HueSharp.Converters;
using Newtonsoft.Json;

namespace HueSharp.Messages.Schedules
{
    [JsonConverter(typeof(GetAllSchedulesResponseConverter))]
    public class GetAllSchedulesResponse : List<GetScheduleResponse>, IHueResponse { }
}
