using HueSharp.Converters;
using HueSharp.Enums;
using Newtonsoft.Json;

using System;

namespace HueSharp.Messages.Schedules
{
    public class GetScheduleResponse : IHueResponse, IContainsCommand
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "localtime"), JsonConverter(typeof(ScheduleTimingConverter))]
        public ScheduleTiming Timing { get; set; }
        [JsonProperty(PropertyName = "status")]
        public ScheduleStatus Status { get; set; }
        [JsonProperty(PropertyName = "autodelete")]
        public bool AutoDelete { get; set; }
        public bool ShouldSerializeAutoDelete() => !Timing.Type.HasFlag(ScheduleTimingTypes.Recurring);
        [JsonProperty(PropertyName = "command")]
        public Command Command { get; set; }
        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        public bool ShouldSerializeCreated() => false;
    }
}
