using Weber.Core.Results;

namespace Weber.Core.Controllers
{
    public class BaseController
    {
        public OkResult OkResult(object result)
        {
            return Results.OkResult.Create(result);
        }

        public BadResult BadResult(object result)
        {
            return Results.BadResult.Create(result);
        }
    }
}
