using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HueSharp.Messages.Sensors
{
    public class GenericTemperatureSensor : SensorBase
    {
        public GenericTemperatureSensor()
        {
            Type = "ZLLTemperature";
        }

        public GenericTemperatureSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "ZLLTemperature";
                Configuration = jObject.SelectToken("config").ToObject<GenericTemperatureSensorConfiguration>();
                State = jObject.SelectToken("state").ToObject<GenericTemperatureSensorState>();
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GenericTemperatureSensorConfiguration
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
        public bool Ledindication { get; set; }
        [JsonProperty("usertest")]
        public bool Usertest { get; set; }
        [JsonProperty("pending")]
        public object[] Pending { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GenericTemperatureSensorState
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
        [JsonProperty("lastupdated")]
        public DateTime LastUpdated { get; set; }
    }
}