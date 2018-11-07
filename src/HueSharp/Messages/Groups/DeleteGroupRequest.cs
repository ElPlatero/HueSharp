using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    public class DeleteGroupRequest : HueRequestBase
    {
        public int GroupId { get; set; }
        private bool _isAcknowledged;

        public DeleteGroupRequest() : this(-1) { }
        public DeleteGroupRequest(int groupId) : base("groups", HttpMethod.Delete)
        {
            GroupId = groupId;
        }

        public override string Address
        {
            get
            {
                if (GroupId < 0) throw new ArgumentException("GroupId must be set before attempting to delete a group.");
                if (!_isAcknowledged) throw new ArgumentException("Due to safety reasons, the delete request can not be used before you used its Acknowledge()-Method.");
                return $"{base.Address}/{GroupId}";
            }
        }

        public void Acknowledge() => _isAcknowledged = true;

        protected override IHueResponse Deserialize(string json) => JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
    }
}
