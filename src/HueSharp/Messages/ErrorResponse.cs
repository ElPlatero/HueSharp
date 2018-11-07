using HueSharp.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HueSharp.Messages
{
    [JsonConverter(typeof(ErrorResponseConverter))]
    public class ErrorResponse : List<ErrorMessage>, IHueResponse
    {
    }
}
