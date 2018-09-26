using HueSharp.Enums;
using System;
using System.Linq;
using Xunit;

namespace HueSharp.Tests
{
    class ExtensionsTest
    {
        private const string DEV_USER = "hRls7hTDQwox8oCu0GT-rDlY2rdzo7BWgDfmBzh4";

        public void ToDescriptionTest()
        {
            Enum.GetValues(typeof(ErrorCode)).OfType<ErrorCode>().ToList().ForEach(
                p =>
                {
                    Console.WriteLine("Checking {0} - {1} - {2}", (int)p, p, p.ToDescription());
                    Assert.True(p.ToString() != p.ToDescription());
                });
        }
    }
}
