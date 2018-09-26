using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HueSharp.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GroupType
    {
        LightGroup = 1,
        Room = 2,
        Luminaire = 3,
        LightSource = 4
    }
}
