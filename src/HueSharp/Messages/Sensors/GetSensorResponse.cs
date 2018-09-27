namespace HueSharp.Messages.Sensors
{
    public class GetSensorResponse : IHueResponse
    {
        public SensorBase Sensor { get; }

        public GetSensorResponse(SensorBase sensor)
        {
            Sensor = sensor;
        }
    }
}
