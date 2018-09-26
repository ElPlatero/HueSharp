using HueSharp.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HueSharp.Messages.Sensors
{
    [JsonConverter(typeof(GetAllSensorsResponseConverter))]
    public class GetAllSensorsResponse : List<SensorBase>, IHueResponse { }
}
