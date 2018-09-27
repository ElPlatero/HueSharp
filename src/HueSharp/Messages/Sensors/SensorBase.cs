using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HueSharp.Messages.Sensors
{
    public abstract class SensorBase
    {
        protected SensorBase() { }
        protected SensorBase(JObject jObject)
        {
            if (jObject != null)
            {
                Name = jObject.SelectToken("name").ToString();
                Type = jObject.SelectToken("type").ToString();
                ModelId = jObject.SelectToken("modelid").ToString();
                ManufacturerName = jObject.SelectToken("manufacturername").ToString();
                SoftwareVersion = jObject.SelectToken("swversion") == null ? string.Empty : jObject.SelectToken("swversion").ToString();
                UniqueHardwareId = jObject.SelectToken("uniqueid") == null ? string.Empty : jObject.SelectToken("uniqueid").ToString();
            }
        }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }
        [JsonProperty(PropertyName = "modelid")]
        public string ModelId { get; set; }
        [JsonProperty(PropertyName = "manufacturername")]
        public string ManufacturerName { get; set; }
        [JsonProperty(PropertyName = "swversion")]
        public string SoftwareVersion { get; set; }
        [JsonProperty(PropertyName = "uniqueid")]
        public string UniqueHardwareId { get; set; }

        [JsonProperty(PropertyName = "state")]
        public object State { get; set; }
        public bool ShouldSerializeState() => State != null;

        [JsonProperty(PropertyName = "config")]
        public object Configuration { get; set; }

        public virtual JObject GetJObject()
        {
            return JObject.FromObject(this, new JsonSerializer());
        }

        public static SensorBase Create(JObject jObject)
        {
            switch ((string)jObject.SelectToken("type"))
            {
                case "Daylight": return new DaylightSensor(jObject);
                case "ZGPSwitch": return new HueTapSensor(jObject);
                case "ZLLSwitch": return new HueDimmerSensor(jObject);
                case "CLIPGenericStatus": return new GenericStatusSensor(jObject);
                case "ZLLTemperature": return new GenericTemperatureSensor(jObject);
                case "ZLLPresence": return new GenericPresenceSensor(jObject);
                case "ZLLLightLevel": return new GenericLightLevelSensor(jObject);
                default: throw new ArgumentException($"Unknown type <{jObject.SelectToken("type")}>");
            }
        }
    }
}
