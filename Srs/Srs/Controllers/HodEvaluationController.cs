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
        [HttpGet("HasHodEvaluation/{evaluationId}")]
        public async Task<IActionResult> HasHodEvaluation(int evaluationId)
        {
            try
            {
                var hasEvaluation = await _hodEvaluationService.HasHodEvaluationAsync(evaluationId);
                return Ok(new { evaluationId, hasHodEvaluation = hasEvaluation });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }
        [HttpGet("HodEvaluationByEvaluationId/{evaluationId}")]
        public async Task<IActionResult> GetHodEvaluationByEvaluationId(int evaluationId)
        {
            try
            {
                var result = await _hodEvaluationService.GetHodEvaluationAsync(evaluationId);
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
        [HttpPost("ReturnToProfessor")]
        public async Task<IActionResult> ReturnToProfessor([FromBody] ReturnToProfessor dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var success = await _hodEvaluationService.ReturnToProfessorAsync(dto);
                if (success)
                    return Ok(new { message = "Evaluation returned to professor successfully" });
                else
                    return StatusCode(500, new { message = "Failed to return evaluation to professor" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while returning the evaluation to professor", details = ex.Message });
            }
        }
        [HttpPost("ReturnToTA")]
        public async Task<IActionResult> ReturnToTA([FromBody] ReturnEvaluationHODDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var success = await _hodEvaluationService.ReturnToTaAsync(dto);
                if (success)
                    return Ok(new { message = "Evaluation returned to TA successfully" });
                else
                    return StatusCode(500, new { message = "Failed to return evaluation to TA" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while returning the evaluation to TA", details = ex.Message });
            }
        }
        [HttpGet("GetTAsForHOD")]
        public async Task<IActionResult> GetTAsForHOD([FromQuery] int periodId,[FromQuery] int hodDepartmentId,[FromQuery] DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 GetTAsForHOD called - Period: {periodId}, Department: {hodDepartmentId}");
                var result = await _hodEvaluationService.GetTAsForHODAsync(periodId, hodDepartmentId, startDate);
                Console.WriteLine($"✅ Returning {result.Count} TAs");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetTAsForHOD: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}