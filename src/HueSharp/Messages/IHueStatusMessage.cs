using HueSharp.Messages.Lights;

namespace HueSharp.Messages
{
    public interface IHueStatusMessage
    {
        LightState Status { get; set; }
    }
}