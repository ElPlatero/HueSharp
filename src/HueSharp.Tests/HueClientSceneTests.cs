using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Scenes;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientSceneTests : TestBase, IDisposable
    {
        public HueClientSceneTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            _sceneId = CreateTemporaryScene();
        }

        private readonly string _sceneId;

        [ExplicitFact]
        public void GetAllScenesTest()
        {
            IHueRequest request = new GetAllScenesRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetAllScenesResponse, "response is of correct type");
            OnLog(response);
        }

        [ExplicitFact]
        public void GetSceneTest()
        {
            IHueRequest request = new GetSceneRequest(_sceneId);

            var response = _client.GetResponse(request);
            Assert.True(response is GetSceneResponse, "response is of correct type");

            ((GetSceneResponse)response).LightStates.ToList().ForEach(p =>
            {
                OnLog($"{p.LightId}");
                OnLog(p.ToString());
            });
        }

        [ExplicitFact]
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

            OnLog(_client.GetResponse(request));
            Assert.True(!string.IsNullOrEmpty(request.Parameters.SceneId));
            DeleteTemporaryScene(request.Parameters.SceneId);
        }

        [ExplicitFact]
        public void DeleteSceneTest()
        {
            var id = CreateTemporaryScene();

            var response = _client.GetResponse(new DeleteSceneRequest(id));
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void ModifySceneTest()
        {
            IHueRequest request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                Name = "new Name",
                LightIds = new [] { 7 },
                UseCurrentStatus = true
            });

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void ModifySceneLightState()
        {
            IHueRequest request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                LightIds = new[] { 7 },
                UseCurrentStatus = true
            });

            _client.GetResponse(request);

            request = new ModifySceneLightStateRequest(_sceneId, 7)
            {
                LightState = new LightState
                {
                    IsOn = true,
                    Effect = LightEffect.ColorLoop
                }
            };

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
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
            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            return request.Parameters.SceneId;
        }

        private void DeleteTemporaryScene(string id)
        {
            var response = _client.GetResponse(new DeleteSceneRequest(id));
            Assert.True(response is SuccessResponse);
        }

        public void Dispose()
        {
            DeleteTemporaryScene(_sceneId);
        }
    }
}
