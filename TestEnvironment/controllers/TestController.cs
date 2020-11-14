using System;
using System.Threading.Tasks;
using TestEnvironment.dto;
using Weber.Core.Attributes;
using Weber.Core.Controllers;
using Weber.Core.Inputs;
using Weber.Core.Results;

namespace TestEnvironment.controllers
{
    [Controller()]
    public class TestController : BaseController
    {
        private readonly TestService _testService;
        public TestController(TestService testService)
        {
            _testService = testService;
        }

        ///////////////////////////////////////////////////////////
        /////////////////////// GETs /////////////////////////////

        [Route()]
        [HttpGet()]
        public async Task<IMethodResult> GetHelloWorld()
        {
            try
            {
                var result = _testService.PrintHelloWorld();
                return OkResult(result);
            }
            catch (Exception e)
            {
                return BadResult($"TestController GetHelloWorld Error: {e.Message}");
            }
        }

        [Route("WithNumber")]
        [HttpGet()]
        public async Task<IMethodResult> GetModifyAndReturn([FromQuery] string number)
        {
            try
            {
                return OkResult($"Result: {Int32.Parse(number) + 1}");
            }
            catch (Exception e)
            {
                return BadResult($"TestController GetModifyAndReturn Error: {e.Message}");
            }
        }

        [Route("camelCaseRoute")]
        [HttpGet()]
        public async Task<IMethodResult> GetModifyAndReturnCamel()
        {
            try
            {
                return OkResult($"It's Route With camel case");
            }
            catch (Exception e)
            {
                return BadResult($"TestController GetModifyAndReturnCamel Error: {e.Message}");
            }
        }

        [Route("{routeParameter}")]
        [HttpGet()]
        public async Task<IMethodResult> GetModifyAndReturn([FromQuery] string queryParameter, [FromRoute] string routeParameter)
        {
            try
            {
                return OkResult($"Result: {queryParameter + routeParameter}");
            }
            catch (Exception e)
            {
                return BadResult($"TestController GetModifyAndReturnWithParameter Error: {e.Message}");
            }
        }

        ///////////////////////////////////////////////////////////
        /////////////////////// POSTs /////////////////////////////

        [Route()]
        [HttpPost]
        public async Task<IMethodResult> PostReturnValue([FromBody] InputDataForPostMethod input)
        {
            try
            {
                return OkResult($"My name is {input.Name} and i {input.Age} years old");
            }
            catch (Exception e)
            {
                return BadResult($"TestController PostReturnValue Error: {e.Message}");
            }
        }

        [Route("WithRoute")]
        [HttpPost]
        public async Task<IMethodResult> PostReturnValueWithRoute([FromBody] InputDataForPostMethod input)
        {
            try
            {
                return OkResult($"My name is {input.Name} and i {input.Age} years old and I love methods with routes");
            }
            catch (Exception e)
            {
                return BadResult($"TestController PostReturnValueWithRoute Error: {e.Message}");
            }
        }

        ///////////////////////////////////////////////////////////
        /////////////////////// PUTs  /////////////////////////////

        [Route()]
        [HttpPut]
        public async Task<IMethodResult> PutTheName()
        {
            try
            {
                return OkResult("Put is Ok!");
            }
            catch (Exception e)
            {
                return BadResult($"TestController PutTheName Error: {e.Message}");
            }
        }

        [Route("WithRoute")]
        [HttpPut]
        public async Task<IMethodResult> PutTheName([FromBody] InputDataForPostMethod input)
        {
            try
            {
                return OkResult($"Put the name: {input.Name}");
            }
            catch (Exception e)
            {
                return BadResult($"TestController PutTheName Error: {e.Message}");
            }
        }

        ///////////////////////////////////////////////////////////
        /////////////////////// DELETEs  /////////////////////////////

        [Route()]
        [HttpDelete]
        public async Task<IMethodResult> DeleteTheName()
        {
            try
            {
                return OkResult("Name was removed");
            }
            catch (Exception e)
            {
                return BadResult($"TestController DeleteTheName Error: {e.Message}");
            }
        }

        [Route("WithRoute")]
        [HttpDelete]
        public async Task<IMethodResult> DeleteTheNameWithRoute()
        {
            try
            {
                return OkResult("Name was removed: With Route");
            }
            catch (Exception e)
            {
                return BadResult($"TestController DeleteTheNameWithRoute Error: {e.Message}");
            }
        }

        [Route("{param}")]
        [HttpDelete]
        public async Task<IMethodResult> DeleteTheNameWithParameter([FromRoute] string param)
        {
            try
            {
                return OkResult($"{param} was removed: With Parameter");
            }
            catch (Exception e)
            {
                return BadResult($"TestController DeleteTheNameWithParameter Error: {e.Message}");
            }
        }
    }
}
