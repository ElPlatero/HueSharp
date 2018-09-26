using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HueSharp.Messages.Sensors
{
    public class GenericStatusSensor : SensorBase, IConnectedLightingSensor
    {
        public GenericStatusSensor() { Type = "CLIPGenericStatus"; }
        public GenericStatusSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "CLIPGenericStatus";
                Configuration = jObject.SelectToken("config").ToObject<GenericStatusSensorConfiguration>();
                State = jObject.SelectToken("state").ToObject<GenericStatusSensorState>();
            }
        }
    }

    public class GenericStatusSensorConfiguration
    {
        [JsonProperty(PropertyName = "on")]
        public bool IsOn { get; set; }
        [JsonIgnore, JsonProperty(PropertyName = "reachable")]
        public bool IsReachable { get; set; }
        [JsonProperty(PropertyName = "battery")]
        public int BatteryLevel { get; set; }
        public bool ShouldSerializeBatteryLevel() => BatteryLevel > 0;
        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }
        public bool ShouldSerializeUrl() => Url != null;
    }

    public class GenericStatusSensorState
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }
}
