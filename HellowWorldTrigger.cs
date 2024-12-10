using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MCT.Models;

namespace MCT.Functions
{
    public class HellowWorldTrigger
    {
        private readonly ILogger<HellowWorldTrigger> _logger;

        public HellowWorldTrigger(ILogger<HellowWorldTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HellowWorldTrigger")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("CalculatorTrigger")]
        public async Task<IActionResult> Calculator(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calculator")] HttpRequest req) {
                var request = await req.ReadFromJsonAsync<CalculationRequest>();
                
                if (request == null || string.IsNullOrEmpty(request.Operation)) {
                    return new BadRequestObjectResult("Invalid request");
                }
            int result = 0;
            switch (request.Operation.ToLower())
            {
                case "add":
                    result = request.A + request.B;
                    break;
                case "subtract":
                    result = request.A - request.B;
                    break;
                case "multiply":
                    result = request.A * request.B;
                    break;
                case "divide":
                    if (request.B == 0)
                        return new BadRequestObjectResult("Division by zero is not allowed.");
                    result = request.A / request.B;
                    break;
                default:
                    return new BadRequestObjectResult("Invalid operation. Supported operations are: add, subtract, multiply, divide.");
            }

            return new OkObjectResult(new { result, operation = request.Operation });
        }
    }
}