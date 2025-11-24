using Business_Access.Interfaces;
using Business_Access.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.HODEvaluation;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HodEvaluationController : ControllerBase
    {
        private readonly IHODEvaluation _hodEvaluationService;
        public HodEvaluationController(IHODEvaluation hodEvaluationService)
        {
            _hodEvaluationService = hodEvaluationService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateHodEvaluation([FromBody] CreateHodEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var hodEvalId = await _hodEvaluationService.CreateHodEvaluationAsync(dto);
                return Ok(new { HodEvalId = hodEvalId, Message = "HOD evaluation created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the HOD evaluation", details = ex.Message });
            }
        }
        [HttpPut("UpdateHodEvaluation/{evaluationId}")]
        public async Task<IActionResult> UpdateHodEvaluation(int evaluationId, [FromBody] UpdateHodEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _hodEvaluationService.UpdateHodEvaluationAsync(evaluationId, dto);
                return Ok(new { message = "HOD evaluation updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the HOD evaluation", details = ex.Message });
            }
        }
        [HttpGet("HodEvaluationByEvaluationId/{evaluationId}")]
        public async Task<IActionResult> GetHodEvaluationByEvaluationId(int evaluationId)
        {
            try
            {
                var result = await _hodEvaluationService.GetHodEvaluationByEvaluationIdAsync(evaluationId);
                if (result == null)
                    return NotFound(new { message = $"No HOD evaluation found for evaluation ID {evaluationId}" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the HOD evaluation", details = ex.Message });
            }
        }
        [HttpGet("GetHodEvaluationByPeriod/{periodId}")]
        public async Task<IActionResult> GetHodEvaluationsByPeriod(int periodId)
        {
            try
            {
                var results = await _hodEvaluationService.GetHodEvaluationsByPeriodAsync(periodId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving HOD evaluations", details = ex.Message });
            }
        }
    }
}
