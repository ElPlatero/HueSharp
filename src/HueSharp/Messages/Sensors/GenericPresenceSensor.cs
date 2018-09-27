using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HueSharp.Messages.Sensors
{
    public class GenericPresenceSensor : SensorBase
    {
        public GenericPresenceSensor()
        {
            Type = "ZLLPresence";
        }

        public GenericPresenceSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "ZLLPresence";
                Configuration = jObject.SelectToken("config").ToObject<GenericPresenceSensorConfiguration>();
                State = jObject.SelectToken("state").ToObject<GenericPresenceSensorState>();
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GenericPresenceSensorConfiguration
    {
        [JsonProperty("on")]
        public bool On { get; set; }
        [JsonProperty("battery")]
        public int Battery { get; set; }
        [JsonProperty("reachable")]
        public bool Reachable { get; set; }
        [JsonProperty("alert")]
        public string Alert { get; set; }
        [JsonProperty("ledindication")]
        public bool LedIndication { get; set; }
        [JsonProperty("usertest")]
        public bool Usertest { get; set; }
        [JsonProperty("sensitivity")]
        public int Sensitivity { get; set; }
        [JsonProperty("sensitivitymax")]
        public int MaxSensitivity { get; set; }
        [JsonProperty("pending")]
        public object[] Pending { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GenericPresenceSensorState
    {
        [JsonProperty("presence")]
        public bool Presence { get; set; }
        [JsonProperty("lastupdated")]
        public DateTime LastUpdated { get; set; }
    }
}