using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Lights;
using HueSharp.Messages.Scenes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientSceneTests : TestBase, IDisposable
    {
        public HueClientSceneTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            CreateTemporaryScene().Wait();
        }

        private string _sceneId;

        [ExplicitFact]
        public async Task GetAllScenesTest()
        {
            IHueRequest request = new GetAllScenesRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetAllScenesResponse, "response is of correct type");
            OnLog(response);
        }

        [ExplicitFact]
        public async Task GetSceneTest()
        {
            IHueRequest request = new GetSceneRequest(_sceneId);

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetSceneResponse, "response is of correct type");

            ((GetSceneResponse)response).LightStates.ToList().ForEach(p =>
            {
                OnLog($"{p.LightId}");
                OnLog(p.ToString());
            });
        }

        [ExplicitFact]
        public async Task CreateSceneTest()
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

            OnLog(await _client.GetResponseAsync(request));
            Assert.True(!string.IsNullOrEmpty(request.Parameters.SceneId));
            DeleteTemporaryScene(request.Parameters.SceneId).Wait();
        }

        [ExplicitFact]
        public async Task DeleteSceneTest()
        {
            var backup = _sceneId;
            CreateTemporaryScene().Wait();

            var response = await _client.GetResponseAsync(new DeleteSceneRequest(_sceneId));
            Assert.True(response is SuccessResponse);
            OnLog(response);
            _sceneId = backup;
        }

        [ExplicitFact]
        public async Task ModifySceneTest()
        {
            IHueRequest request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                Name = "new Name",
                LightIds = new [] { 7 },
                UseCurrentStatus = true
            });

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task ModifySceneLightState()
        {
            IHueRequest request = new ModifySceneRequest(new ModifySceneParameters
            {
                SceneId = _sceneId,
                LightIds = new[] { 7 },
                UseCurrentStatus = true
            });

            _client.GetResponseAsync(request).Wait();

            request = new ModifySceneLightStateRequest(_sceneId, 7)
            {
                LightState = new LightState
                {
                    IsOn = true,
                    Effect = LightEffect.ColorLoop
                }
            };

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        private Task CreateTemporaryScene()
        {
            var request = new CreateSceneRequest
            {
                Parameters = new CreateSceneParameters
                {
                    Name = "tmp scene",
                    LightIds = new[] { 2, 3, 4 }
                }
            };
            return _client.GetResponseAsync(request).ContinueWith(p =>
            {
                var response = p.Result;
                Assert.True(response is SuccessResponse);
                _sceneId = request.Parameters.SceneId;
            });
        }

        private async Task DeleteTemporaryScene(string id)
        {
            var response = await _client.GetResponseAsync(new DeleteSceneRequest(id));
            Assert.True(response is SuccessResponse);
        }

        public void Dispose()
        {
            DeleteTemporaryScene(_sceneId).Wait();
        }
    }
}
