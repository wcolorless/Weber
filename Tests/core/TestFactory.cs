using System;
using System.Collections.Generic;
using System.Text;
using TestEnvironment.controllers;
using Weber.Core.Server;

namespace Tests.core
{
    public class TestFactory
    {
        private static WeberServer _server;
        public static WeberServer GetServerAndStart()
        {
            if (_server != null) return _server;
            var weber = WeberServer
                .Init("http://localhost:3000/", new WeberServerSettings()
                {
                    InitControllersFromAssembly = false
                })
                .AddService<ITestService, TestService>()
                .AddController<TestController>()
                .Go();
            _server = weber;
            return weber;
        }
    }
}
