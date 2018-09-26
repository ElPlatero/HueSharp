using System;

namespace HueSharp
{
    public interface ILoggable
    {
        event EventHandler<string> Log;
    }
}
