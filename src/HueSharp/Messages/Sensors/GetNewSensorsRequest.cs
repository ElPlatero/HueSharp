using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Messages.Sensors
{
    public class GetNewSensorsRequest : HueRequestBase
    {
        public GetNewSensorsRequest() : base("sensors/new", HttpMethod.Get) { }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<GetNewSensorsResponse>(json);
        }
    }
}
