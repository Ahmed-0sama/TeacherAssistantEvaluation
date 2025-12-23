using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IHRService _hrService;
        public StatisticsController(IHRService hrService)
        {
            _hrService = hrService;
        }
        [HttpGet("EvaluationStatistics/{evaluationPeriod}")]
        public async Task<IActionResult> GetEvaluationStatistics(int evaluationPeriod)
        {
            try
            {
                var statistics = await _hrService.GetEvaluationStatisticsAsync(evaluationPeriod);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving evaluation statistics.", details = ex.Message });
            }
        }
    }
}
