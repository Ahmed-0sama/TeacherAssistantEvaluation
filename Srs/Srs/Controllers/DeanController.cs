using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.DeanDto;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeanController : ControllerBase
    {
        private readonly IDean _deanService;
        public DeanController(IDean deanService)
        {
            _deanService = deanService;
        }
        [HttpGet("evaluationDetail/{evaluationId}")]
        public async Task<IActionResult> GetEvaluationDetail(int evaluationId)
        {
            try
            {
                var result = await _deanService.GetEvaluationDetailAsync(evaluationId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpPost("acceptEvaluation")]
        public async Task<IActionResult> AcceptEvaluation([FromBody] AcceptEvaluationDto dto)
        {
            try
            {
                var result = await _deanService.AcceptEvaluationAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("returnEvaluation")]
        public async Task<IActionResult> ReturnEvaluation([FromBody] ReturnEvaluationDto dto)
        {
            try
            {
                var result = await _deanService.ReturnEvaluationAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("updateEvaluationCriteria")]
        public async Task<IActionResult> UpdateEvaluationCriteria([FromBody] UpdateDeanEvaluationDto dto)
        {
            try
            {
                Console.WriteLine($"🔵 Dean Controller - Received update request");
                Console.WriteLine($"   - EvaluationId: {dto.EvaluationId}");
                Console.WriteLine($"   - Criterion Count: {dto.CriterionRatings?.Count ?? 0}");
                Console.WriteLine($"   - CreatedByUserId: {dto.CreatedByUserId}");

                if (dto == null)
                {
                    Console.WriteLine("❌ DTO is null");
                    return BadRequest("Request body is empty");
                }

                if (dto.CriterionRatings == null || !dto.CriterionRatings.Any())
                {
                    Console.WriteLine("❌ No criterion ratings provided");
                    return BadRequest("No criterion ratings provided");
                }

                var result = await _deanService.UpdateEvaluationCriteriaAsync(dto);

                if (result.Success)
                {
                    Console.WriteLine($"✅ Update successful: {result.Message}");
                    return Ok(result);
                }

                Console.WriteLine($"⚠️ Update failed: {result.Message}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in controller: {ex.Message}");
                Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new DeanActionResponseDto
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}",
                    EvaluationId = dto?.EvaluationId ?? 0
                });
            }
        }
        [HttpGet("GetTAsForDean")]
        public async Task<IActionResult> GetTAsForDean(
            [FromQuery] int periodId,
            [FromQuery] int deanDepartmentId,
            [FromQuery] DateOnly startDate)
        {
            try
            {
                Console.WriteLine($"📡 GetTAsForDean called - Period: {periodId}, Department: {deanDepartmentId}");
                var result = await _deanService.GetTAsForDeanAsync(periodId, deanDepartmentId, startDate);
                Console.WriteLine($"✅ Returning {result.Count} TAs");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetTAsForDean: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
