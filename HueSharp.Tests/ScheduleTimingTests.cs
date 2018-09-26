using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Groups;
using HueSharp.Messages.Schedules;
using HueSharp.Net;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HueSharp.Tests
{
    [TestFixture]
    class ScheduleTimingTests
    {

        [Test]
        [TestCaseSource("CreateTimerTestCases")]
        public void CreateScheduleTimingTest(string serializedTimer, DateTime expectedDateTime, TimeSpan expectedRandomOffset, int expectedLoops, int expectedType, int expectedWeekdays)
        {
            var result = ScheduleTiming.Create(serializedTimer);
            Assert.AreEqual(result.BaseDate, expectedDateTime, "date comparison");
            Assert.AreEqual(result.RandomizedOffSet, expectedRandomOffset, "random offset comparison");
            Assert.AreEqual(result.Loops, expectedLoops, "loop count comparison");
            Assert.AreEqual((int)result.Type, expectedType, "timing type comparison");
            Assert.AreEqual((int)result.Weekdays, expectedWeekdays, "weekdays comparison");
            Assert.AreEqual(serializedTimer, result.ToJson(), "serialized vs reserialized comparison");
        }

        public static IEnumerable<TestCaseData> CreateTimerTestCases
        {
            get
            {
                yield return new TestCaseData("1976-04-03T09:05:59", new DateTime(1976, 4, 3, 9, 5, 59), default(TimeSpan), 0, 1, 0).SetName("Absolute time");
                yield return new TestCaseData("1976-04-03T09:05:59A05:30:59", new DateTime(1976, 4, 3, 9, 5, 59), new TimeSpan(5,30,59), 0, 5, 0).SetName("Randomized time");
                yield return new TestCaseData("W124/T09:00:00", DateTime.Now.Date.Add(new TimeSpan(9,0,0)), default(TimeSpan), 0, 9, 124).SetName("Recurring times");
                yield return new TestCaseData("W124/T09:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(9, 0, 0)), new TimeSpan(5, 30, 59), 0, 13, 124).SetName("Recurring randomized times");
                yield return new TestCaseData("PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), 0, 2, 0).SetName("Timer, expiring after a given time");
                yield return new TestCaseData("PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), 0, 6, 0).SetName("Timer, with random element");
                yield return new TestCaseData("R/PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), -1, 10, 0).SetName("Recurring Timer, endless");
                yield return new TestCaseData("R15/PT20:00:00", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), default(TimeSpan), 15, 10, 0).SetName("Recurring Timer, limited");
                yield return new TestCaseData("R/PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), -1, 14, 0).SetName("Recurring randomized timer, endless");
                yield return new TestCaseData("R17/PT20:00:00A05:30:59", DateTime.Now.Date.Add(new TimeSpan(20, 0, 0)), new TimeSpan(5, 30, 59), 17, 14, 0).SetName("Recurring randomized timer, limited");
            }
        }
    }
}
