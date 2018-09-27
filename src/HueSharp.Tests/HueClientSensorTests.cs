using HueSharp.Messages;
using HueSharp.Messages.Sensors;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class HueClientSensorTests : TestBase, IDisposable
    {
        public HueClientSensorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
            _tmpSensorId = CreateTmpSensor();
        }

        #region Hue Test Setup/TearDown
        private readonly int _tmpSensorId;

        private int CreateTmpSensor()
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

            _client.GetResponse(request);
            return request.Sensor.Id;
        }

        private void DeleteTmpSensor()
        {
            OnLog("Teardown: deleteing temporary sensors");
            IHueRequest request = new GetAllSensorsRequest();
            var tempSensors = ((GetAllSensorsResponse) _client.GetResponse(request)).Where(p => p.ModelId == "TMPSENSOR").ToList();

            foreach (var sensor in tempSensors)
            {
                request = new DeleteSensorRequest(sensor.Id);
                var response = _client.GetResponse(request);
                Assert.True(response is SuccessResponse);
                OnLog(response);
            }
            OnLog("Teardown complete.");
        }

        public void Dispose()
        {
            DeleteTmpSensor();
        }
        #endregion

        #region tests
        [ExplicitFact]
        public void GetAllSensorsTest()
        {
            IHueRequest request = new GetAllSensorsRequest();

            var response = _client.GetResponse(request);

            Assert.True(response is GetAllSensorsResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void CreateSensorTest()
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

            var response = _client.GetResponse(request);

            Assert.True(response is SuccessResponse);
            OnLog(response);
            Assert.True(((CreateSensorRequest)request).Sensor.Id > 0);
        }

        [ExplicitFact]
        public void FindNewSensorsTest()
        {
            IHueRequest request = new FindNewSensorsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void GetNewSensorsTest()
        {
            IHueRequest request = new GetNewSensorsRequest();

            var response = _client.GetResponse(request);
            Assert.True(response is GetNewSensorsResponse);
        }

        [ExplicitFact]
        public void GetSensorTest()
        {
            IHueRequest request = new GetSensorRequest(1);

            var response = _client.GetResponse(request);

            Assert.True(response is GetSensorResponse);
            Assert.True(((GetSensorResponse)response).Sensor is DaylightSensor);
            Assert.Equal(@"PHDL00", ((GetSensorResponse)response).Sensor.ModelId);
        }

        [ExplicitFact]
        public void UpdateSensorTest()
        {
            IHueRequest request = new UpdateSensorRequest(2, "Schalter Schlafzimmer");

            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void ChangeSensorConfigTest()
        {
            var sensor = ((GetSensorResponse)_client.GetResponse(new GetSensorRequest(_tmpSensorId))).Sensor;
            ((GenericStatusSensorConfiguration)sensor.Configuration).IsOn = false;

            var request = new ChangeSensorConfigRequest(sensor);
            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }

        [ExplicitFact]
        public void ChangeSensorStateTest()
        {
            var sensor = ((GetSensorResponse)_client.GetResponse(new GetSensorRequest(_tmpSensorId))).Sensor;
            ((GenericStatusSensorState)sensor.State).Status = 20;

            IHueRequest request = new ChangeSensorStateRequest(sensor);
            var response = _client.GetResponse(request);
            Assert.True(response is SuccessResponse);
            OnLog(response);
        }
        #endregion
    }
}
