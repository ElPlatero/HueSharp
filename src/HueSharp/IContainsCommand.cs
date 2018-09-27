using HueSharp.Messages;

namespace HueSharp
{
    interface IContainsCommand
    {
        Command Command { get; }
    }
}
