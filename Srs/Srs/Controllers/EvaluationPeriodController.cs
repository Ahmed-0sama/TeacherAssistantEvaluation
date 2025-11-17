using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.EvaluationPeriod;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationPeriodController : ControllerBase
    {
        private readonly IEvaluationPeriod _evaluationPeriodService;
        public EvaluationPeriodController(IEvaluationPeriod evaluationPeriod)
        {
            _evaluationPeriodService = evaluationPeriod;
        }
        [HttpPost("CreateEvaluationPeriod")]
        public async Task<IActionResult> CreatePeriod([FromBody] CreateEvaluationPeriodDto periodDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var periodId = await _evaluationPeriodService.CreateEvaluationPeriodAsync(periodDto);

                return Ok(new { id = periodId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPeriodById(int id)
        {
            try
            {
                var period = await _evaluationPeriodService.GetEvaluationPeriodByIdAsync(id);
                return Ok(period);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("GetActiveEvaluationPeriod")]
        public async Task<IActionResult> GetActiveEvaluationPeriod()
        {
            var period = await _evaluationPeriodService.GetCurrentEvaluationPeriodAsync();
            if (period == null)
            {
                return NotFound(new { message = "No active evaluation period found." });
            }
            return Ok(period);
        }
        [HttpGet("GetAllEvaluationPeriods")]
        public async Task<IActionResult> GetAllPeriods()
        {
            try
            {
                var periods = await _evaluationPeriodService.GetAllEvaluationPeriodsAsync();
                return Ok(periods);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving periods", details = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePeriod(int id, [FromBody] CreateEvaluationPeriodDto periodDto)
        {
            // Validate model state (checks data annotations)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _evaluationPeriodService.UpdateEvaluationPeriodAsync(id, periodDto);

                return Ok(new
                {
                    message = "Evaluation period updated successfully",
                    periodId = id
                });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An error occurred while updating the evaluation period",
                        details = ex.Message
                    }
                 );
            }
        }
        [HttpPost("{id}/Close")]
        public async Task<IActionResult> ClosePeriod(int id)
        {
            try
            {
                await _evaluationPeriodService.CloseEvaluationPeriodAsync(id);

                return Ok(new
                {
                    message = "Evaluation period closed successfully",
                    periodId = id,
                    closedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1)
                });
            }
            catch (Exception ex)
            {
                // 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An error occurred while closing the evaluation period",
                        details = ex.Message
                    }
                );
            }
        }
    }
}