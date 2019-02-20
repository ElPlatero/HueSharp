using HueSharp.Messages.Lights;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace HueSharp.Messages
{
    public class Command : HueRequestBase, IUploadable
    {
        public Command() : base("", HttpMethod.Get) { }
        public Command(IHueRequest request) : base(request.Address, request.Method)
        {
            if(!(request is IHueStatusMessage statusRequest)) throw new InvalidOperationException("Request base for a command must implement IHueStatusMessage!");
            Body = statusRequest.Status;
        }

        [JsonProperty(PropertyName = "address")]
        public string CompleteAddress { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string HttpVerb
        {
            get => Method.ToString().ToUpper();
            set => Method = new HttpMethod(value);
        }

        [JsonProperty(PropertyName = "body")]
        public LightState Body { get; set; }

        public string GetRequestBody()
        {
            throw new JsonException("Unexpected call. This class should never be used to actually perform a request.");
        }

        protected override IHueResponse Deserialize(string json)
        {
            throw new JsonException("Unexpected call. This class should never be used to actually perform a request.");
        }
    }
}
