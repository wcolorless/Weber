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
    public class TestHttpPostMethods
    {
        [Fact]
        public void TestAllPostMethods()
        {
            var sever = TestFactory.GetServerAndStart();
            var bodyObj = new InputDataForPostMethod
            {
                Name = "Weber",
                Age = 35
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bodyObj));
            var response1 = RequestActivator.SendRequest(RequestType.POST, "testcontroller/", data, "").Result;
            Assert.Equal($"My name is {bodyObj.Name} and i {bodyObj.Age} years old", response1 as string);
            var response2 = RequestActivator.SendRequest(RequestType.POST, "testcontroller/WithRoute", data, "").Result;
            Assert.Equal($"My name is {bodyObj.Name} and i {bodyObj.Age} years old and I love methods with routes", response2 as string);
        }
    }
}
