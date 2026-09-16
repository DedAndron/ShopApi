using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestCTController(ILogger<TestCTController> _logger) : ControllerBase
    {
        [HttpGet("without-ct")]
        public async Task<IActionResult> TestWithoutCT()
        {
            _logger.LogInformation("TestWithoutCT called");
            await Task.Delay(5000); // Simulate some work
            _logger.LogInformation("Action 1 completed");
            await Task.Delay(5000); // Simulate some work
            _logger.LogInformation("Action 2 completed");
            _logger.LogInformation("TestWithoutCT completed");
            return Ok("TestWithoutCT");
        }
        [HttpGet("with-ct")]
        public async Task<IActionResult> TestWithCT(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("TestWithCT called");
                await Task.Delay(5000, cancellationToken); // Simulate some work
                _logger.LogInformation("Action 1 completed");
                await Task.Delay(5000, cancellationToken); // Simulate some work
                _logger.LogInformation("Action 2 completed");
                _logger.LogInformation("TestWithCT completed");
            }
            catch(OperationCanceledException ex)
            {
                
            }
            return Ok("TestWithoutCT");
        }
    }
}
