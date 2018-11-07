using HueSharp.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HueSharp.Messages.Lights
{
    [JsonConverter(typeof(GetAllLightsResponseConverter))]
    class GetAllLightsResponse : List<Light>, IHueResponse
    {
        public void ResetAllStates()
        {
            ForEach(p => p.Status.Reset());
        }
    }
}
