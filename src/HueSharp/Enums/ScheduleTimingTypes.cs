using System;

namespace HueSharp.Enums
{
    [Flags]
    public enum ScheduleTimingTypes
    {
        Alarm = 1,
        Timer = 2,
        Randomized = 4,
        Recurring = 8
    }
}
