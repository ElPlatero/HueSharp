using HueSharp.Messages;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HueSharp.Net
{
    public class HueClient
    {
        private readonly ILogger _logger;
        public Uri BaseAddress { get; set; }
        public string User { get; }

        public HueClient(ILoggerFactory loggerFactory, string user) : this(loggerFactory, user, (Uri)null) { }
        public HueClient(ILoggerFactory loggerFactory, string user, string address) : this(loggerFactory, user, new Uri(address)) { }
        public HueClient(ILoggerFactory loggerFactory, string user, IPAddress address) : this(loggerFactory, user, new Uri($"http://{address}")) { }
        public HueClient(ILoggerFactory loggerFactory, string user, Uri baseAddress)
        {
            _logger = loggerFactory == null ? new NullLogger<HueClient>() : loggerFactory.CreateLogger<HueClient>();
            LogTrace("Hue client created.");

            User = user;
            BaseAddress = baseAddress;
        }

        public async Task<IHueResponse> GetResponseAsync(IHueRequest hueRequest)
        {
            if (hueRequest is IContainsCommand setsCommand)
            {
                setsCommand.Command.CompleteAddress = $"/api/{User}/{setsCommand.Command.Address}";
            }

            var uri = GetRequestUri(hueRequest.Address);

            LogTrace(@"Preparing {method}-request to ""{uri}"".", hueRequest.Method.Method, uri);
            var requestMessage = new HttpRequestMessage(hueRequest.Method, uri);


            if (hueRequest is IUploadable uploadableRequest)
            {
                var requestContent = uploadableRequest.GetRequestBody();
                LogTrace($"Preparing json to send: {Environment.NewLine}{requestContent}");
                requestMessage.Content = new StringContent(requestContent, Encoding.UTF8);
            }
            else
            {
                requestMessage.Content = new StringContent(string.Empty);
            }

            using (var client = GetClient())
            using (var response = await client.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    LogTrace(responseString);
                    return hueRequest.GetResponse(responseString);
                }

                LogError($"Error getting response from hub, HttpStatus {response.StatusCode}");
                LogDebug(await response.Content.ReadAsStringAsync());
                throw new HttpRequestException();
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private Uri GetRequestUri(string relativePath) => new Uri(BaseAddress, $"api/{User}/{relativePath}");

        private void LogTrace(string message, params object[] parms) => _logger.LogTrace(message, parms);
        private void LogDebug(string message, params object[] parms) => _logger.LogDebug(message, parms);
        private void LogWarning(string message, params object[] parms) => _logger.LogWarning(message, parms);
        private void LogError(string message, params object[] parms) => _logger.LogError(message, parms);
        private void LogCritical(string message, params object[] parms) => _logger.LogCritical(message, parms);

    }
}
