using HueSharp.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HueSharp.Messages.Schedules
{
    public class ScheduleTiming
    {
        private ScheduleTiming() { }

        public DateTime BaseDate { get; set; }
        public TimeSpan RandomizedOffSet { get; set; }
        public ScheduleTimingTypes Type { get; set; }
        public TimingWeekdays Weekdays { get; set; }
        public int Loops { get; set; }

        public string ToJson()
        {
            var sb = new StringBuilder();
            if (Weekdays > 0) sb.AppendFormat("W{0:000}/T{1:c}", (int)Weekdays, BaseDate.TimeOfDay);
            else if (Type.HasFlag(ScheduleTimingTypes.Alarm)) sb.AppendFormat("{0:s}", BaseDate);

            else if (Type.HasFlag(ScheduleTimingTypes.Recurring)) sb.AppendFormat("R{0}/", Loops > 0 ? Loops.ToString("00") : string.Empty);
            if (Type.HasFlag(ScheduleTimingTypes.Timer)) sb.AppendFormat("PT{0:c}", BaseDate.TimeOfDay);

            if (Type.HasFlag(ScheduleTimingTypes.Randomized)) sb.AppendFormat("A{0:c}", RandomizedOffSet);

            return sb.ToString();
        }


        public static ScheduleTiming CreateNew(ScheduleTimingTypes type)
        {
            return new ScheduleTiming { Type = type };
        }

        public static ScheduleTiming Create(string serializedTiming)
        {
            var groups = new Regex(@"(?<datetime>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})?(W(?<weekdays>\d{3}))?(\/T(?<time>\d{2}:\d{2}:\d{2}))?(?<recTimer>R(?<loop>\d*))?(\/*PT(?<timer>\d{2}:\d{2}:\d{2}))?(A(?<random>\d{2}:\d{2}:\d{2}))?").Matches(serializedTiming).OfType<Match>().First(p => p.Length > 0).Groups;

            var result = new ScheduleTiming();
            if (groups["datetime"].Success && !string.IsNullOrEmpty(groups["datetime"].Value))
            {
                result.BaseDate = DateTime.ParseExact(groups["datetime"].Value, "s", CultureInfo.InvariantCulture);
                result.Type |= ScheduleTimingTypes.Alarm;
            }
            if (groups["weekdays"].Success && !string.IsNullOrEmpty(groups["weekdays"].Value) && groups["time"].Success && !string.IsNullOrEmpty(groups["time"].Value))
            {
                result.Type |= ScheduleTimingTypes.Alarm;
                result.Type |= ScheduleTimingTypes.Recurring;
                result.Weekdays = (TimingWeekdays)Convert.ToInt32(groups["weekdays"].Value);
                result.BaseDate = DateTime.Now.Date.Add(TimeSpan.ParseExact(groups["time"].Value, "c", CultureInfo.InvariantCulture));
            }
            if (groups["recTimer"].Success && !string.IsNullOrEmpty(groups["recTimer"].Value))
            {
                result.Type |= ScheduleTimingTypes.Timer;
                result.Type |= ScheduleTimingTypes.Recurring;
                result.Loops = string.IsNullOrEmpty(groups["loop"].Value) ? -1 : int.Parse(groups["loop"].Value);
            }
            if (groups["timer"].Success && !string.IsNullOrEmpty(groups["timer"].Value))
            {
                result.Type |= ScheduleTimingTypes.Timer;
                result.BaseDate = DateTime.Now.Date.Add(TimeSpan.ParseExact(groups["timer"].Value, "c", CultureInfo.InvariantCulture));
            }
            if (groups["random"].Success && !string.IsNullOrEmpty(groups["random"].Value))
            {
                result.Type |= ScheduleTimingTypes.Randomized;
                result.RandomizedOffSet = TimeSpan.ParseExact(groups["random"].Value, "c", CultureInfo.InvariantCulture);
            }

            return result;
        }

    }
}
