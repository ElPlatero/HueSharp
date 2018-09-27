using System.Collections.Generic;
using HueSharp.Converters;
using Newtonsoft.Json;

namespace HueSharp.Messages.Scenes
{
    [JsonConverter(typeof(GetAllScenesResponseConverter))]
    public class GetAllScenesResponse : List<GetSceneResponse>, IHueResponse { }
}
