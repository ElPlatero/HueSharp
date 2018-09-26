using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Sensors
{
    public class CreateSensorRequest : HueRequestBase, IUploadable
    {
        public SensorBase Sensor { get; set; }

        public CreateSensorRequest() : base("sensors", HttpMethod.Post) { }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(Sensor);
        }

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            Sensor.Id = Convert.ToInt32(result["id"]);
            return result;
        }
    }
}
