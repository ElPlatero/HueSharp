using System;

namespace HueSharp.Enums
{
    [Flags]
    public enum TimingWeekdays
    {
        Monday = 64,
        Tuesday = 32,
        Wednesday = 16,
        Thursday = 8,
        Friday = 4,
        Saturday = 2,
        Sunday = 1
    }
}
