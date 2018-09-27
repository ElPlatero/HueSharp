using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Messages
{
    public interface IHueRequest : ILoggable
    {
        HttpMethod Method { get; }
        string Address { get; }
        IHueResponse GetResponse(string json);
    }
}
