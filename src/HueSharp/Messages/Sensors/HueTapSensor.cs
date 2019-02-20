using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HueSharp.Enums;
using HueSharp.Converters;

namespace HueSharp.Messages.Sensors
{
    public class HueTapSensor : SensorBase
    {
        public string UniqueId { get; set; }

        public HueTapSensor() { Type = "ZGPSwitch"; }
        public HueTapSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "ZGPSwitch";
                Configuration = jObject.SelectToken("config").ToObject<HueTapConfiguration>();
                State = jObject.SelectToken("state").ToObject<HueTapSensorState>();
                UniqueId = jObject.SelectToken("uniqueid").ToString();
            }
        }
    }

    public class HueTapConfiguration
    {
        [JsonProperty(PropertyName = "on")]
        public bool IsOn { get; set; }
    }

    public class HueTapSensorState
    {
        [JsonIgnore]
        public HueButtonState ButtonState
        {
            get => (HueButtonState)_buttonState;
            set => _buttonState = (int)value;
        }

        [JsonProperty(PropertyName = "buttonevent")]
        private int _buttonState;

        [JsonProperty(PropertyName = "lastupdated"), JsonConverter(typeof(UtcDateTimeConverter))]
        public DateTime LastUpdate { get; set; }

    }
}
