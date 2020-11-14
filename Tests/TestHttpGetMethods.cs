using System;
using Tests.core;
using Weber.Core.Controllers;
using Xunit;

namespace Tests
{
    public class TestHttpGetMethods
    {
        [Fact]
        public void TestAllGetMethods()
        {
            var sever = TestFactory.GetServerAndStart();
            var response1 = RequestActivator.SendRequest(RequestType.GET, "testcontroller/", null, "").Result;
            Assert.Equal("Hello world", response1 as string);
            var response2 = RequestActivator.SendRequest(RequestType.GET, "testcontroller/WithNumber?number=123", null, "").Result;
            Assert.Equal("Result: 124", response2 as string);
            var response3 = RequestActivator.SendRequest(RequestType.GET, "testcontroller/camelCaseRoute", null, "").Result;
            Assert.Equal("It's Route With camel case", response3 as string);
            var response4 = RequestActivator.SendRequest(RequestType.GET, "testcontroller/67890?queryParameter=012345", null, "").Result;
            Assert.Equal("Result: 01234567890", response4 as string);
        }
    }
}
