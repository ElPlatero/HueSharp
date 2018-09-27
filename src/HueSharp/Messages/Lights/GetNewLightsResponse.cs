using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HueSharp.Messages.Lights
{
    [JsonConverter(typeof(GetNewLightsResponseConverter))]
    public class GetNewLightsResponse: List<BasicLight>, IHueResponse
    {
        [JsonProperty(PropertyName = "lastscan"), JsonConverter(typeof(LastScanDateTimeConverter))]
        public DateTime LastScan { get; set; }
    }
}
    