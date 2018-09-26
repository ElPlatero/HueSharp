using HueSharp.Converters;
using HueSharp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    public class SetGroupRequest : HueRequestBase, IUploadable
    {
        [JsonIgnore]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string NewName { get; set; }
        public bool ShouldSerializeNewName() => !string.IsNullOrEmpty(NewName);

        [JsonProperty(PropertyName = "lights"), JsonConverter(typeof(IntAsStringConverter))]
        public List<int> Lights { get; set; }
        public bool ShouldSerializeLights() => Lights.Any();

        [JsonProperty(PropertyName = "class")]
        public RoomClass Class { get; set; }
        public bool ShouldSerializeClass() => Class != RoomClass.Other;

        public SetGroupRequest() : this(0, null, RoomClass.Other) { }
        public SetGroupRequest(GetGroupResponse response) : this(response.Id, response.LightIds, response.Class) { }
        public SetGroupRequest(int groupId, IEnumerable<int> lightIds, RoomClass roomClass) : base("groups", HttpMethod.Put)
        {
            GroupId = groupId;
            Lights = new List<int>(lightIds);
            Class = roomClass;
        }

        public override string Address => $"{base.Address}/{GroupId}";

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
