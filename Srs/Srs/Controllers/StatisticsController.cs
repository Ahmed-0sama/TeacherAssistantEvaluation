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
        [HttpGet("EvaluationStatistics")]
        public async Task<IActionResult> GetEvaluationStatistics(
            [FromQuery] int evaluationPeriod,
            [FromQuery] int hrDepartmentId,
            [FromQuery] DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 GetEvaluationStatistics called - Period: {evaluationPeriod}");
                var statistics = await _hrService.GetEvaluationStatisticsAsync(
                    evaluationPeriod,
                    hrDepartmentId,
                    startDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetEvaluationStatistics: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetAllTAsForHR")]
        public async Task<IActionResult> GetAllTAsForHR([FromQuery] int periodId,[FromQuery] int hrDepartmentId,[FromQuery] DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 GetAllTAsForHR called - Period: {periodId}, Department: {hrDepartmentId}");
                var result = await _hrService.GetAllTAsForHRAsync(periodId, hrDepartmentId, startDate);
                Console.WriteLine($"✅ Returning {result.Count} TAs");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetAllTAsForHR: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
