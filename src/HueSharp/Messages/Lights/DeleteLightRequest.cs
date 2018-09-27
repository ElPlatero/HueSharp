using HueSharp.Converters;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages.Lights
{
    public class DeleteLightRequest : HueRequestBase
    {
        public int LightId { get; set; }
        private bool _isInitialized = false;

        public DeleteLightRequest(int lightId) : base("lights", HttpMethod.Delete) { LightId = lightId; }
        public DeleteLightRequest() : this(-1) { }

        public override string Address
        {
            get
            {
                if (LightId < 0) throw new ArgumentException("LightId must be set before attempting to delete a light.");
                if (!_isInitialized) throw new ArgumentException("Due to safety reasons, the delete request can not be used before you used its Acknowledge()-Method.");
                return $"{base.Address}/{LightId}";
            }
        }

        public void Acknowledge()
        {
            _isInitialized = true;
        }

        protected override IHueResponse Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SuccessResponse>(json, new SuccessResponseConverter(Address));
        }
    }
}
