using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyDI;
using Weber.Core.Controllers;
using Weber.Core.Routes;

namespace Weber.Core.Server
{
    public class WeberServer
    {
        private Dictionary<string,Type> _controllers { get; set; } = new Dictionary<string, Type>();
        private List<ControllerInfo> _controllerInfos = new List<ControllerInfo>();
        private List<RouteInfo> _routes { get; set; } = new List<RouteInfo>();
        private WeberServerSettings _weberServerSettings;
        private readonly string _baseAddress;
        private readonly TinyDependencyInjection _tinyDependencyInjection;
        private HttpServer _httpServer;
        private HandlerDispatcher _handlerDispatcher;


        public bool IsActive => _httpServer.IsActive;

        private WeberServer(string baseAddress, WeberServerSettings weberServerSettings)
        {
            _baseAddress = baseAddress;
            _weberServerSettings = weberServerSettings;
            _tinyDependencyInjection = new TinyDependencyInjection();
        }
        public static WeberServer Init(string baseAddress, WeberServerSettings weberServerSettings)
        {
            return new WeberServer(baseAddress, weberServerSettings);
        }

        public WeberServer AddService<Interface, Implementation>(bool isSingletonService = false)
        {
            _tinyDependencyInjection.AddDependency(new Dependency()
                .For<Interface>()
                .Use<Implementation>()
                .SetBehaviour(isSingletonService ? DIBehaviour.Singleton : DIBehaviour.Renewed));
            return this;
        }

        public WeberServer AddController<TController>()
        {
            var type = typeof(TController);
            _controllers.TryAdd(type.Name.ToLower(), type);
            return this;
        }

        /// <summary>
        /// Launches the server
        /// </summary>
        /// <returns></returns>
        public WeberServer Go()
        {
            try
            {
                if(!_controllers.Any()) throw new Exception("WeberServer Error: Controller list is empty");
                _controllerInfos.Clear();
                _controllerInfos.AddRange(_controllers.Select(x => ControllerInfo.GetControllerInfo(x.Value)).ToList());
                CheckingRules.CheckRoutes(_controllerInfos);
                _routes.Clear();
                _routes.AddRange(_controllerInfos.SelectMany(x => x.Routes));
                _handlerDispatcher = new HandlerDispatcher(_controllers, _tinyDependencyInjection, _controllerInfos, _routes);
                _httpServer = new HttpServer(_baseAddress, _controllers, _tinyDependencyInjection, _controllerInfos, _routes);
                _httpServer.StartServer();
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}");
            }
            return this;
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        /// <returns></returns>
        public WeberServer Stop()
        {
            if (_httpServer != null && _httpServer.IsActive)
            {
                _httpServer.StopServer();
            }
            return this;
        }
    }
}
