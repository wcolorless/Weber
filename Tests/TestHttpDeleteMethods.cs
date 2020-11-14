using System;
using System.Collections.Generic;
using System.Text;
using Tests.core;
using Weber.Core.Controllers;
using Xunit;

namespace Tests
{
    public class TestHttpDeleteMethods
    {
        [Fact]
        public void TestAllDeleteMethods()
        {
            var sever = TestFactory.GetServerAndStart();
            var response1 = RequestActivator.SendRequest(RequestType.DELETE, "testcontroller/", null, "").Result;
            Assert.Equal("Name was removed", response1 as string);
            var response2 = RequestActivator.SendRequest(RequestType.DELETE, "testcontroller/WithRoute", null, "").Result;
            Assert.Equal("Name was removed: With Route", response2 as string);
            var response3 = RequestActivator.SendRequest(RequestType.DELETE, "testcontroller/MyName", null, "").Result;
            Assert.Equal("MyName was removed: With Parameter", response3 as string);
        }
    }
}
