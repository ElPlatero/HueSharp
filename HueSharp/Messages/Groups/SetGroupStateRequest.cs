using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    public class SetGroupStateRequest : HueRequestBase, IUploadable
    {
        public int GroupId { get; set; }
        public SetGroupState NewState { get; set; }

        public SetGroupStateRequest() : this(0, new SetGroupState()) { }
        public SetGroupStateRequest(int groupId, SetGroupState newState) : base("groups", HttpMethod.Put)
        {
            GroupId = groupId;
            NewState = newState;
        }

        public override string Address => $"{base.Address}/{GroupId}/action";

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(NewState);
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }

    }
}
