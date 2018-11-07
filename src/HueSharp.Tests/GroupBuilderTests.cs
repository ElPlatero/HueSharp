using System.Net.Http;
using HueSharp.Builder;
using HueSharp.Enums;
using Xunit;
using Xunit.Abstractions;

namespace HueSharp.Tests
{
    public class GroupBuilderTests : TestBase
    {
        public GroupBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void BuildGetAllGroupsRequestTest()
        {
            var request = HueRequestBuilder.Select.Groups.Build();

            Assert.Equal("groups", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void BuildGetGroupRequestTest()
        {
            var request = HueRequestBuilder.Select.Group(1).Build();

            Assert.Equal("groups/1", request.Address);
            Assert.Equal(HttpMethod.Get, request.Method);
        }

        [Fact]
        public void ModifyNameTest()
        {
            const string NAME = "my Name";

            var request = HueRequestBuilder.Modify.Group(1).Attributes.UseTheseLights(1, 2, 3, 4, 5).Name(NAME).Build();

            Assert.Equal("groups/1", request.Address);
            AssertRequestBody(request, ("name", NAME));
        }

        [Fact]
        public void ModifyClassTest()
        {
            var request = HueRequestBuilder.Modify.Group(1).Attributes.UseTheseLights(1).Class(RoomClass.Recreation).Build();

            Assert.Equal("groups/1", request.Address);
            AssertRequestBody(request, ("class", "Recreation"));
        }
    }
}
