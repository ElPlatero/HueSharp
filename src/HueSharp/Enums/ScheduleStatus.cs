using HueSharp.Converters;
using Newtonsoft.Json;
using System.ComponentModel;

namespace HueSharp.Enums
{
    [JsonConverter(typeof(DescriptionConverter))]
    public enum ScheduleStatus
    {
        [Description("enabled")]
        Enabled = 1,
        [Description("disabled")]
        Disabled = 2
    }
}
