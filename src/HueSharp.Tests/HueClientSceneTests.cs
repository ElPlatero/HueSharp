using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Scenes;
using HueSharp.Net;
using System;
using System.Linq;
using Xunit;

namespace HueSharp.Tests
{
    class HueClientSceneTests
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.100.14";

        private HueClient _client;
        private string _sceneId;
        private void ClientOnLog(object sender, string e) => Console.WriteLine(e);

        public void Setup()
        {
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            _sceneId = CreateTemporaryScene();
            _client.Log += ClientOnLog;

        }

        public void TearDown()
        {
            _client.Log -= ClientOnLog;
            DeleteTemporaryScene(_sceneId);
        }


        public void GetAllScenesTest()
        {
            var request = new GetAllScenesRequest();
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetAllScenesResponse, "response is of correct type");
            ((GetAllScenesResponse)response).ForEach(p => Console.WriteLine($"{p.Id} - \"{p.Name}\" ({string.Join(",", p.LightIds.ToArray())}): {p.AppData.Version}"));
        }

        public void GetSceneTest()
        {
            var request = new GetSceneRequest(_sceneId);
            IHueResponse response = null;

            response = _client.GetResponse(request);
            Assert.True(response is GetSceneResponse, "response is of correct type");

            ((GetSceneResponse)response).LightStates.ToList().ForEach(p => { Console.WriteLine($"{p.LightId}"); Console.WriteLine(p); });
        }

        public void CreateSceneTest()
        {
            var request = new CreateSceneRequest
            {
                Parameters = new CreateSceneParameters
                {
                    Name = "tmp scene",
                    AppData = new SceneApplicationData
                    {
                        Data = "temporary scene",
                        Version = 1
                    },
                    LightIds = new[] { 2, 3, 4 },
                    TransitionTime = TimeSpan.FromMilliseconds(100)
                }
            };

            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(!string.IsNullOrEmpty(request.Parameters.SceneId));

            DeleteTemporaryScene(request.Parameters.SceneId);
        }

        public void DeleteSceneTest()
        {
            string id = CreateTemporaryScene();

            IHueResponse response = null;
            response = _client.GetResponse(new DeleteSceneRequest(id));
            Assert.True(response is SuccessResponse);
        }

        public void ModifySceneTest()
        {
            var request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                Name = "new Name",
                LightIds = new [] { 7 },
                UseCurrentStatus = true
            });

            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }

        public void ModifySceneLightState()
        {
            IHueRequest request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                LightIds = new[] { 7 },
                UseCurrentStatus = true
            });

            IHueResponse response = null;
            response = _client.GetResponse(request);

            request = new ModifySceneLightStateRequest(_sceneId, 7)
            {
                LightState = new LightState
                {
                    IsOn = true,
                    Effect = LightEffect.ColorLoop
                }
            };

            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
        }

        private string CreateTemporaryScene()
        {
            var request = new CreateSceneRequest
            {
                Parameters = new CreateSceneParameters
                {
                    Name = "tmp scene",
                    LightIds = new[] { 2, 3, 4 }
                }
            };
            IHueResponse response = null;
            response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            return request.Parameters.SceneId;
        }

        private void DeleteTemporaryScene(string id)
        {
            IHueResponse response = null;
            response = _client.GetResponse(new DeleteSceneRequest(id));
            Assert.True(response is SuccessResponse);
        }
    }
}
