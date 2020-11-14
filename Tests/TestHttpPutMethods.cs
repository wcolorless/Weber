using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TestEnvironment.dto;
using Tests.core;
using Weber.Core.Controllers;
using Xunit;

namespace Tests
{
    public class TestHttpPutMethods
    {
        [Fact]
        public void TestAllPutMethods()
        {
            var sever = TestFactory.GetServerAndStart();
            var bodyObj = new InputDataForPostMethod
            {
                Name = "Weber",
                Age = 35
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bodyObj));
            var response1 = RequestActivator.SendRequest(RequestType.PUT, "testcontroller/", new byte[]{}, "").Result;
            Assert.Equal("Put is Ok!", response1 as string);
            var response2 = RequestActivator.SendRequest(RequestType.PUT, "testcontroller/WithRoute", data, "").Result;
            Assert.Equal($"Put the name: {bodyObj.Name}", response2 as string);
        }
    }
}
