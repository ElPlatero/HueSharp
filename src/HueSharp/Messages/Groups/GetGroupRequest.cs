using HueSharp.Enums;
using Newtonsoft.Json;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    class GetGroupRequest : HueRequestBase
    {
        public int GroupId { get; set; }

        public GetGroupRequest() : this(0) { }
        public GetGroupRequest(int groupId) : base("groups", HttpMethod.Get) { GroupId = groupId; }

        public override string Address => $"{base.Address}/{GroupId}";

        protected override IHueResponse Deserialize(string json)
        {
            var result = JsonConvert.DeserializeObject<GetGroupResponse>(json);
            result.Id = GroupId;
            if (result.Class == 0) result.Class = RoomClass.Other;
            return result;
        }
    }
}
