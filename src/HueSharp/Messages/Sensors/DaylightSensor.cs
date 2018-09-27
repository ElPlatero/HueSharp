using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using HueSharp.Converters;

namespace HueSharp.Messages.Sensors
{
    public class DaylightSensor : SensorBase
    {
        public DaylightSensor() : base() { Type = "Daylight"; }
        public DaylightSensor(JObject jObject) : base(jObject)
        {
            if (jObject != null)
            {
                Type = "Daylight";
                Configuration = jObject.SelectToken("config").ToObject<DaylightSensorConfiguration>();
                State = jObject.SelectToken("state").ToObject<DaylightSensorState>();
            }
        }
    }

    public class DaylightSensorConfiguration
    {
        [JsonProperty(PropertyName = "on")] public bool IsOn { get; set; }

        private GeoCoordinate _sensorPosition = new GeoCoordinate();

        [JsonProperty(PropertyName = "long")]
        private string Longitude
        {
            get
            {
                if (_sensorPosition.IsUnknown) return "none";
                return string.Format("{0}{1}", Math.Abs(_sensorPosition.Longitude).ToString("000.0000", CultureInfo.InvariantCulture), _sensorPosition.Longitude < 0 ? "W" : "E");
            }
            set
            {
                if (value.Equals("none")) return;
                _sensorPosition.Longitude = double.Parse(value.Substring(0, value.Length-1), CultureInfo.InvariantCulture);
                if (value.EndsWith("W")) _sensorPosition.Longitude *= -1;
            }
        }

        [JsonProperty(PropertyName = "lat")]
        private string Latitude
        {
            get
            {
                if (_sensorPosition.IsUnknown) return "none";
                return string.Format("{0}{1}", Math.Abs(_sensorPosition.Latitude).ToString("000.0000", CultureInfo.InvariantCulture), _sensorPosition.Longitude < 0 ? "S" : "N");
            }
            set
            {
                if (value.Equals("none")) return;
                _sensorPosition.Latitude = double.Parse(value.Substring(0, value.Length - 1), CultureInfo.InvariantCulture);
                if (value.EndsWith("S")) _sensorPosition.Latitude *= -1;
            }
        }
        [JsonIgnore]
        public TimeSpan SunriseOffset { get; set; }
        [JsonIgnore]
        public TimeSpan SunsetOffset { get; set; }

        [JsonProperty(PropertyName = "sunriseoffset")]
        private int SunriseOffsetMinutes
        {
            get
            {
                return Convert.ToInt32(SunriseOffset.TotalMinutes);
            }
            set
            {
                SunriseOffset = TimeSpan.FromMinutes(value);
            }
        }

        [JsonProperty(PropertyName = "sunsetoffset")]
        private int SunsetOffsetMinutes
        {
            get
            {
                return Convert.ToInt32(SunsetOffset.TotalMinutes);
            }
            set
            {
                SunsetOffset = TimeSpan.FromMinutes(value);
            }
        }
        [JsonIgnore]
        public GeoCoordinate Position
        {
            get { return _sensorPosition; }
            set { _sensorPosition = value; }
        }
    }
    public class DaylightSensorState
    {
        [JsonProperty(PropertyName = "daylight")]
        public bool? IsDaylight { get; set; }

        [JsonProperty(PropertyName = "lastupdated"), JsonConverter(typeof(UtcDateTimeConverter))]
        public DateTime LastUpdate { get; set; }
    }
}
