using HueSharp.Messages;

namespace HueSharp.Builder
{
    public interface IBuilder
    {
        IHueRequest Build();
    }
}