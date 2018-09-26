using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HueSharp.Enums;
using System;

namespace HueSharp.Messages.Sensors
{
    public class HueDimmerSensor : SensorBase
    {
        public string UniqueId { get; set; }

        public HueDimmerSensor() { Type = "ZLLSwitch"; }
        public HueDimmerSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "ZLLSwitch";
                Configuration = jObject.SelectToken("config").ToObject<HueDimmerConfiguration>();
                State = jObject.SelectToken("state").ToObject<HueDimmerState>();
                UniqueId = jObject.SelectToken("uniqueid").ToString();
            }
        }
    }

    public class HueDimmerConfiguration
    {
        [JsonProperty(PropertyName = "on")]
        public bool IsOn { get; set; }

        [JsonProperty(PropertyName = "battery")]
        public bool IsBatteryOperated { get; set; }

        [JsonProperty(PropertyName = "reachable")]
        public bool IsInRange { get; set; }
    }

    public class HueDimmerState
    {
        [JsonIgnore]
        public HueButtonState ButtonState
        {
            get { return (HueButtonState)_buttonState; }
            set { _buttonState = (int)value; }
        }

        [JsonProperty(PropertyName = "buttonevent")]
        private int _buttonState;
        
    }
}
