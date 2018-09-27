using HueSharp.Messages.Schedules;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace HueSharp.Tests
{
    public class ScheduleTimingTests
    {
        [Theory]
        [ClassData(typeof(ScheduleTimingTestCaseFactory))]
        public void CreateScheduleTimingTest(string serializedTimer, DateTime expectedDateTime, TimeSpan expectedRandomOffset, int expectedLoops, int expectedType, int expectedWeekdays)
        {
            var result = ScheduleTiming.Create(serializedTimer);
            Assert.Equal(result.BaseDate, expectedDateTime);
            Assert.Equal(result.RandomizedOffSet, expectedRandomOffset);
            Assert.Equal(result.Loops, expectedLoops);
            Assert.Equal((int)result.Type, expectedType);
            Assert.Equal((int)result.Weekdays, expectedWeekdays);
            Assert.Equal(serializedTimer, result.ToJson());
        }

        class ScheduleTimingTestCaseFactory : IEnumerable<object[]>
        {
            private static IEnumerable<object[]> CreateTimerTestCases
            {
                get
                {
                    yield return new object[] { "1976-04-03T09:05:59", new DateTime(1976, 4, 3, 9, 5, 59), default(TimeSpan), 0, 1, 0 };
                    yield return new object[] { "1976-04-03T09:05:59A05:30:59", new DateTime(1976, 4, 3, 9, 5, 59), new TimeSpan(5, 30, 59), 0, 5, 0 };
                    yield return new object[] { "W124/T09:00:00", DateTime.Now.Date.Add(new TimeSpan(9, 0, 0)), default(TimeSpan), 0, 9, 124 };
                    yield return new object[] { "W124/T09:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(9, 0, 0)), new TimeSpan(5, 30, 59), 0, 13, 124 };
                    yield return new object[] { "PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), 0, 2, 0 };
                    yield return new object[] { "PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), 0, 6, 0 };
                    yield return new object[] { "R/PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), -1, 10, 0 };
                    yield return new object[] { "R15/PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), 15, 10, 0 };
                    yield return new object[] { "R/PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), -1, 14, 0 };
                    yield return new object[] { "R17/PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), 17, 14, 0 };
                }
            }

            public IEnumerator<object[]> GetEnumerator() => CreateTimerTestCases.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        
    }
}
