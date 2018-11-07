using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HueSharp.Messages.Sensors
{
    public class GenericLightLevelSensor : SensorBase
    {
        public GenericLightLevelSensor()
        {
            Type = "ZLLLightLevel";
        }

        public GenericLightLevelSensor(JObject jObject) : base(jObject)
        {
            Type = "ZLLLightLevel";
            Configuration = jObject.SelectToken("config").ToObject<GenericLightLevelSensorConfiguration>();
            State = jObject.SelectToken("state").ToObject<GenericLightLevelSensorState>();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GenericLightLevelSensorConfiguration
    {
        [JsonProperty("on")]
        public bool On { get; set; }
        [JsonProperty("battery")]
        public int Battery { get; set; }
        [JsonProperty("reachable")]
        public bool Reachable { get; set; }
        [JsonProperty("alert")]
        public string Alert { get; set; }
        [JsonProperty("tholddark")]
        public int Tholddark { get; set; }
        [JsonProperty("tholdoffset")]
        public int Tholdoffset { get; set; }
        [JsonProperty("ledindication")]
        public bool Ledindication { get; set; }
        [JsonProperty("usertest")]
        public bool Usertest { get; set; }
        [JsonProperty("pending")]
        public object[] Pending { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class GenericLightLevelSensorState
    {
        [JsonProperty("lightlevel")]
        public int LightLevel { get; set; }
        [JsonProperty("dark")]
        public bool Dark { get; set; }
        [JsonProperty("daylight")]
        public bool Daylight { get; set; }
        [JsonProperty("lastupdated")]
        public DateTime LastUpdated { get; set; }
    }
}