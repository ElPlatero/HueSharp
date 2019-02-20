using Newtonsoft.Json;
using System.Net.Http;
using System;

namespace HueSharp.Messages
{
    public abstract class HueRequestBase : IHueRequest
    {
        protected JsonSerializer _serializer;
        protected readonly string _address;
        protected HttpMethod _method;

        protected HueRequestBase(string address, HttpMethod method)
        {
            _address = address;
            _method = method;
        }

        [JsonIgnore]
        public virtual string Address => _address;

        [JsonIgnore]
        public virtual HttpMethod Method
        {
            get => _method;
            protected set => _method = value;
        }

        public event EventHandler<string> Log;

        public IHueResponse GetResponse(string json)
        {
            OnLog("Response JSON is:");
            OnLog(this, json);
            if (json.Contains("\"error\""))
            {
                throw new HueResponseException(JsonConvert.DeserializeObject<ErrorResponse>(json));
            }
            return Deserialize(json);
        }

        protected void OnLog(object sender, string message)
        {
            Log?.Invoke(sender, message);
        }
        protected void OnLog(string formatString, params object[] parameters)
        {
            OnLog(this, string.Format(formatString, parameters));
        }
        protected abstract IHueResponse Deserialize(string json);
    }
}
