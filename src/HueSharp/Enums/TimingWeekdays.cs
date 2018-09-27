using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
