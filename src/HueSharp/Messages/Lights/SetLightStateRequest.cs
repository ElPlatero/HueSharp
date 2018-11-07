using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    class SetLightStateRequest : HueRequestBase, IHueStatusMessage, IUploadable, IHueResponse
    {
        public int LightId { get; set; }
        public LightState Status { get; set; }

        public SetLightStateRequest(int lightId) : base("lights", HttpMethod.Put)
        {
            LightId = lightId;
            Status = new SetLightState();
        }

        public SetLightStateRequest() : base("lights", HttpMethod.Put)
        {
            Status = new SetLightState();
        }

        public override string Address => $"{base.Address}/{LightId}/state";

        protected override IHueResponse Deserialize(string json)
        {
            var successMessage = JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
            Status.ApplyChanges(successMessage);
            return this;
        }

        public string GetRequestBody()
        {
            var result = JsonConvert.SerializeObject(Status);
            return result;
        }
    }
}
