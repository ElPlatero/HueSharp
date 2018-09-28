using HueSharp.Messages;
using HueSharp.Messages.Sensors;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientSensorTests : TestBase, IDisposable
    {
        public HueClientSensorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            _tmpSensorId = CreateTmpSensorAsync().Result;
        }

        #region Hue Test Setup/TearDown
        private readonly int _tmpSensorId;

        private Task<int> CreateTmpSensorAsync()
        {
            var request = new CreateSensorRequest
            {
                Sensor = new GenericStatusSensor
                {
                    Name = "temp sensor",
                    ModelId = "TMPSENSOR",
                    SoftwareVersion = "1.0",
                    UniqueHardwareId = "1234567890",
                    ManufacturerName = "TMP Sensors, Ltd.",
                    Configuration = new GenericStatusSensorConfiguration
                    {
                        IsOn = true,
                        IsReachable = true
                    }
                }
            };

            return _client.GetResponseAsync(request).ContinueWith(p =>
            {
                p.Wait();
                return request.Sensor.Id;
            });
        }

        private Task DeleteTmpSensorAsync()
        {
            OnLog("Teardown: deleteing temporary sensors");
            IHueRequest request = new GetAllSensorsRequest();

            return _client.GetResponseAsync(request).ContinueWith(getAllSensors =>
            {
                var tempSensors = ((GetAllSensorsResponse) getAllSensors.Result).Where(p => p.ModelId == "TMPSENSOR")
                    .ToList();
                Task.WhenAll(tempSensors.Select(p => _client.GetResponseAsync(new DeleteSensorRequest(p.Id)).ContinueWith(deleteResponse =>
                {
                    var deleteResponseResult = deleteResponse.Result;
                    Assert.True(deleteResponseResult is SuccessResponse);
                    OnLog(deleteResponseResult);
                }))).Wait();
            });
        }

        public void Dispose()
        {
            DeleteTmpSensorAsync().Wait();
        }
        #endregion

        #region tests
        [ExplicitFact]
        public async Task GetAllSensorsTest()
        {
            IHueRequest request = new GetAllSensorsRequest();

            var response = await _client.GetResponseAsync(request);

            Assert.True(response is GetAllSensorsResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task CreateSensorTest()
        {
            IHueRequest request = new CreateSensorRequest
            {
                Sensor = new GenericStatusSensor
                {
                    Name = "temp sensor",
                    ModelId = "TMPSENSOR",
                    SoftwareVersion = "1.0",
                    UniqueHardwareId = "1234567890",
                    ManufacturerName = "TMP Sensors, Ltd.",
                    Configuration = new GenericStatusSensorConfiguration
                    {
                        IsOn = true,
                        IsReachable = true
                    },
                    State = new GenericStatusSensorState
                    {
                        Status = 100
                    }
                }
            };

            var response = await _client.GetResponseAsync(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
            Assert.True(((CreateSensorRequest)request).Sensor.Id > 0);
        }

        [ExplicitFact]
        public async Task FindNewSensorsTest()
        {
            IHueRequest request = new FindNewSensorsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public async Task GetNewSensorsTest()
        {
            IHueRequest request = new GetNewSensorsRequest();

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is GetNewSensorsResponse);
        }

        [ExplicitFact]
        public async Task GetSensorTest()
        {
            IHueRequest request = new GetSensorRequest(1);

            var response = await _client.GetResponseAsync(request);

            Assert.True(response is GetSensorResponse);
            Assert.True(((GetSensorResponse)response).Sensor is DaylightSensor);
            Assert.Equal(@"PHDL00", ((GetSensorResponse)response).Sensor.ModelId);
        }

        [ExplicitFact]
        public async Task UpdateSensorTest()
        {
            IHueRequest request = new UpdateSensorRequest(2, "Schalter Schlafzimmer");

            var response = await _client.GetResponseAsync(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public Task ChangeSensorConfigTest()
        {
            return _client.GetResponseAsync(new GetSensorRequest(_tmpSensorId)).ContinueWith(getSensor =>
            {
                var sensor = ((GetSensorResponse) getSensor.Result).Sensor;
                ((GenericStatusSensorConfiguration) sensor.Configuration).IsOn = false;

                var request = new ChangeSensorConfigRequest(sensor);
                _client.GetResponseAsync(request).ContinueWith(changeSensor =>
                {
                    var response = changeSensor.Result;
                    Assert.True(response is SuccessResponse);
                    OnLog(response);
                });
            });
        }

        [ExplicitFact]
        public Task ChangeSensorStateTest()
        {
            return _client.GetResponseAsync(new GetSensorRequest(_tmpSensorId)).ContinueWith(getSensor =>
            {
                var sensor = ((GetSensorResponse) getSensor.Result).Sensor;
                ((GenericStatusSensorState) sensor.State).Status = 20;

                IHueRequest request = new ChangeSensorStateRequest(sensor);
                _client.GetResponseAsync(request).ContinueWith(changeSensor =>
                {
                    var response = changeSensor.Result;
                    Assert.True(response is SuccessResponse);
                    OnLog(response);
                });
            });
        }
        #endregion
    }
}
