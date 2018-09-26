using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Messages.Sensors
{
    public class GetSensorRequest : HueRequestBase
    {
        public int SensorId { get; set; }

        public GetSensorRequest(int sensorId = -1) : base("sensors", HttpMethod.Get) { SensorId = sensorId; }

        public override string Address => $"{base.Address}/{SensorId}";

        protected override IHueResponse Deserialize(string json)
        {
            var sensor = SensorBase.Create(JObject.Parse(json));
            sensor.Id = SensorId;
            return new GetSensorResponse(sensor);
        }
    }
}
