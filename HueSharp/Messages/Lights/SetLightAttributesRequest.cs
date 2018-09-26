using HueSharp.Converters;
using HueSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    public class SetLightAttributesRequest : HueRequestBase, IUploadable
    {
        public int LightId { get; set; }
        public string NewName { get; set; }

        public SetLightAttributesRequest() : base("lights", HttpMethod.Put) { }
        public SetLightAttributesRequest(int lightId) : base("lights", HttpMethod.Put) { LightId = lightId; }
        public SetLightAttributesRequest(int lightId, string newName) : base("lights", HttpMethod.Put) { LightId = lightId; NewName = newName; }

        public override string Address => $"{base.Address}/{LightId}";

        protected override IHueResponse Deserialize(string json)
        {
            var successMessage = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            if (successMessage.Any(p => p.Key == "name" && p.Value.ToString().Equals(NewName))) return successMessage;
            var error = new ErrorResponse();
            error.Add(new ErrorMessage { Address = Address, Description = "No error returned, but desired name isn't set either.", Type = ErrorCode.InternalError });
            return error;
        }

        public string GetRequestBody()
        {
            if (string.IsNullOrEmpty(NewName)) throw new ArgumentException("New name must not be null or empty.");
            return $"{{\"name\":\"{NewName}\"}}";
        }
    }
}
