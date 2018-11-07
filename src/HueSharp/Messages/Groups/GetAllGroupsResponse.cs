using HueSharp.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HueSharp.Messages.Groups
{
    [JsonConverter(typeof(GetAllGroupsResponseConverter))]
    public class GetAllGroupsResponse : List<GetGroupResponse>, IHueResponse
    {

    }
}
