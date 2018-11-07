using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Messages.Sensors
{
    [JsonConverter(typeof(GetNewSensorsResponseConverter))]
    public class GetNewSensorsResponse : List<BasicSensor>, IHueResponse
    {
        [JsonProperty(PropertyName = "lastscan"), JsonConverter(typeof(LastScanDateTimeConverter))]
        public DateTime LastScan { get; set; }
    }
}
