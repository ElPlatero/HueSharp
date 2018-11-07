using System.Net.Http;

namespace HueSharp.Messages
{
    public interface IHueRequest
    {
        HttpMethod Method { get; }
        string Address { get; }
        IHueResponse GetResponse(string json);
    }
}
