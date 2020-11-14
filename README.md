# Weber ver. 1.0.0
Web API Framework

Weber is a simple Web API framework. It allows you to create CRUD controllers to meet basic API building needs. Inside there is a built-in IoC container for dependency inversion. Many features are similar to ASP.NET.
The framework is very lightweight and at the moment is not loaded with complex functions, therefore it can be used on weak servers.

The set of supported methods:
- GET
- POST
- PUT
- DELETE


Server start example:
```sh
var weber = WeberServer
.Init("http://localhost:3000/", new WeberServerSettings()
{
    InitControllersFromAssembly = false
})
.AddService<ITestService, TestService>()
.AddController<TestController>()
.Go();
```
This is where you add controllers and dependencies using method ```AddController<TestController>()``` 
and ```AddService<ITestService, TestService>()```.


The repository contains an example of using the framework. And also tests to check the health of the server.

Possible innovations in the following versions:
- Possibility of easier configuration of dependencies and a list of executable controllers.
- Server logging.
- Authentication and JWT tokens.
- Load balancer and more flexible work with threads.

