 using HueSharp.Messages;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HueSharp.Net
{
    public class HueClient : ILoggable
    {
        public Uri BaseAddress { get; set; }
        public string User { get; private set; }
        public event EventHandler<string> Log;

        public HueClient(string user) : this(user, (Uri)null) { }
        public HueClient(string user, string address) : this(user, new Uri(address)) { }
        public HueClient(string user, IPAddress address) : this(user, new Uri(string.Format("http://{0}", address.ToString()))) { }
        public HueClient(string user, Uri baseAddress)
        {
            OnLog("Hue client created.");
            User = user;
            BaseAddress = baseAddress;
        }

        public IHueResponse GetResponse(IHueRequest hueRequest)
        {
            var setsCommand = hueRequest as IContainsCommand;
            if(setsCommand != null)
                setsCommand.Command.CompleteAddress = $"/api/{User}/{setsCommand.Command.Address}";


            hueRequest.Log += OnLog;
            OnLog("Preparing {1}-request to \"{0}\".", GetRequestUri(hueRequest.Address), hueRequest.Method.ToString().ToUpper());

            if (hueRequest.Method == HttpMethod.Get)
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(GetRequestUri(hueRequest.Address));
                try
                {
                    using (var response = httpRequest.GetResponse())
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {

                        var result = hueRequest.GetResponse(reader.ReadToEnd());
                        OnLog("Request completed.");
                        return result;
                    }
                }
                catch (WebException ex)
                {
                    OnLog(string.Format("WebException caught: {0}", ex.Message));
                    using (var errorResponse = ex.Response)
                    using (var responseStream = errorResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                    {
                        var errorText = reader.ReadToEnd();
                        OnLog(errorText);
                        throw;
                    }
                }
            }
            else
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetRequestUri(hueRequest.Address));
                request.Method = hueRequest.Method.ToString().ToUpper();

                if (hueRequest is IUploadable)
                {
                    var requestBody = ((IUploadable)hueRequest).GetRequestBody();
                    OnLog("Preparing json to send: {0}{1}", Environment.NewLine, requestBody);
                    var byteArray = Encoding.UTF8.GetBytes(requestBody);

                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";

                    using (var dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        OnLog("Data successfully sent.");
                    }
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        var result = hueRequest.GetResponse(reader.ReadToEnd());
                        OnLog("Request completed.");
                        return result;
                    }
                }
                catch (WebException ex)
                {
                    OnLog(string.Format("WebException caught: {0}", ex.Message));
                    using (var errorResponse = ex.Response)
                    using (var responseStream = errorResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                    {
                        var errorText = reader.ReadToEnd();
                        OnLog(errorText);
                        throw;
                    }
                }
            }
        }

        private Uri GetRequestUri(string relativePath)
        {
            return new Uri(BaseAddress, string.Format("api/{0}/{1}", User, relativePath));
        }

        private void OnLog(object sender, string message)
        {
            Log?.Invoke(this, message);
        }
        private void OnLog(string formatString, params object[] parameters)
        {
            OnLog(this, string.Format(formatString, parameters));
        }
    }
}
