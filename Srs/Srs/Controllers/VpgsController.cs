using Business_Access.Interfaces;
using Business_Access.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.VPGSEvaluation;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VpgsController : ControllerBase
    {
        private readonly IVPGSEvaluation _vpgsService;
        public VpgsController(IVPGSEvaluation vpgsService)
        {
            _vpgsService = vpgsService;
        }
        [HttpGet("GetGTAsForVPGS")]
        public async Task<IActionResult> GetGTAsForVPGS([FromQuery] int periodId, [FromQuery] int SupervisorId, [FromQuery] DateOnly StartDate)
        {
            try
            {
                Console.WriteLine($"📡 GetGTAsForVPGS called with periodId: {periodId}");
                var result = await _vpgsService.GetGTAsForVPGSAsync(periodId,SupervisorId,StartDate);
                Console.WriteLine($"✅ Returning {result.Count} GTAs");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetGTAsForVPGS: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateVpgsEvaluation([FromBody] CreateVpgsEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var vpgsevalId = await _vpgsService.CreateVpgsEvaluationAsync(dto);
                return Ok(new { VpgsevalId = vpgsevalId, Message = "VPGS evaluation created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the VPGS evaluation", details = ex.Message });
            }
        }

        [HttpGet("evaluation/{evaluationId:int}")]  // ✅ Added :int constraint
        public async Task<IActionResult> GetVpgsEvaluationByEvaluationId(int evaluationId)
        {
            try
            {
                var result = await _vpgsService.GetVpgsEvaluationByEvaluationIdAsync(evaluationId);

                if (result == null)
                    return NotFound(new { message = $"No VPGS evaluation found for evaluation ID {evaluationId}" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the VPGS evaluation", details = ex.Message });
            }
        }

        [HttpGet("period/{periodId:int}")]  // ✅ Added :int constraint - THIS WAS CATCHING YOUR ROUTE!
        public async Task<IActionResult> GetVpgsEvaluationsByPeriod(int periodId)
        {
            try
            {
                var results = await _vpgsService.GetVpgsEvaluationsByPeriodAsync(periodId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving VPGS evaluations", details = ex.Message });
            }
        }

        [HttpGet("period/{periodId:int}/existenceMap")]  // ✅ Added :int constraint
        public async Task<IActionResult> GetVpgsExistenceMap(int periodId)
        {
            try
            {
                var results = await _vpgsService.GetVpgsEvaluationsByPeriodAsync(periodId);

                var existenceMap = results.ToDictionary(
                    r => r.EvaluationId,
                    r => true
                );

                return Ok(existenceMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }

        [HttpPut("evaluation/{evaluationId:int}")]  // ✅ Added :int constraint
        public async Task<IActionResult> UpdateVpgsEvaluation(int evaluationId, [FromBody] UpdateVpgsEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _vpgsService.UpdateVpgsEvaluationAsync(evaluationId, dto);
                return Ok(new { message = "VPGS evaluation updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the VPGS evaluation", details = ex.Message });
            }
        }
    }
}
