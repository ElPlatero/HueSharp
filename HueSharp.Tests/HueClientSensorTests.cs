using HueSharp.Messages;
using HueSharp.Messages.Sensors;
using HueSharp.Net;
using System;
using Xunit;

namespace HueSharp.Tests
{
    public class HueClientSensorTests
    {
        #region Hue Test Setup/TearDown
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";
        private const string DEV_ADDRESS = @"http://192.168.100.14";

        private HueClient _client;
        private int _tmpSensorId;

        private void ClientOnLog(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private void CreateTmpSensor()
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

            IHueResponse response = null;
            response = _client.GetResponse(request);
            _tmpSensorId = request.Sensor.Id;
        }
        private void DeleteTmpSensor(int sensorId)
        {
            var request = new DeleteSensorRequest(sensorId);
            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is SuccessResponse);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            _client = new HueClient(DEV_USER, DEV_ADDRESS);
            CreateTmpSensor();
            _client.Log += ClientOnLog;

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _client.Log -= ClientOnLog;
            DeleteTmpSensor(_tmpSensorId);
        }
        #endregion

        public void GetAllSensorsTest()
        {
            var request = new GetAllSensorsRequest();
            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request), "send request");
            Assert.That(response is GetAllSensorsResponse);
        }

        [Test]
        public void CreateSensorTest()
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

            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request), "create tmp sensor");
            Assert.That(response is SuccessResponse);
            Assert.That(request.Sensor.Id > 0);

            DeleteTmpSensor(request.Sensor.Id);
        }

        [Test]
        public void FindNewSensorsTest()
        {
            var request = new FindNewSensorsRequest();
            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is SuccessResponse);
        }

        [Test]
        public void GetNewSensorsTest()
        {
            var request = new GetNewSensorsRequest();

            IHueResponse response = null;
            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is GetNewSensorsResponse);
        }

        [Test]
        public void GetSensorTest()
        {
            var request = new GetSensorRequest(1);

            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is GetSensorResponse);
            Assert.That(((GetSensorResponse)response).Sensor is DaylightSensor);
            Assert.AreEqual(((GetSensorResponse)response).Sensor.ModelId, "PHDL00");
        }

        [Test]
        public void UpdateSensorTest()
        {
            var request = new UpdateSensorRequest(2, "Schalter Schlafzimmer");

            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is SuccessResponse);
        }

        [Test]
        public void ChangeSensorConfigTest()
        {
            var sensor = ((GetSensorResponse)_client.GetResponse(new GetSensorRequest(_tmpSensorId))).Sensor;
            ((GenericStatusSensorConfiguration)sensor.Configuration).IsOn = false;

            var request = new ChangeSensorConfigRequest(sensor);
            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is SuccessResponse);
        }

        [Test]
        public void ChangeSensorStateTest()
        {
            var sensor = ((GetSensorResponse)_client.GetResponse(new GetSensorRequest(_tmpSensorId))).Sensor;
            ((GenericStatusSensorState)sensor.State).Status = 20;

            var request = new ChangeSensorStateRequest(sensor);
            IHueResponse response = null;

            Assert.DoesNotThrow(() => response = _client.GetResponse(request));
            Assert.That(response is SuccessResponse);
        }
    }
}
