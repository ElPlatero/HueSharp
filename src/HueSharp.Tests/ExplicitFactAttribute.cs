using System.Diagnostics;
using Xunit;

namespace HueSharp.Tests
{
    //shout out to Jimmy Bogard, https://lostechies.com/jimmybogard/2013/06/20/run-tests-explicitly-in-xunit-net/
    public sealed class ExplicitFactAttribute : FactAttribute
    {
        public ExplicitFactAttribute()
        {
            if (!Debugger.IsAttached)
            {
                Skip = "Only running in interactive mode.";
            }
        }
    }
}