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
        [HttpGet("GetVpgsById{vpgsevalId}")]
        public async Task<IActionResult> GetVpgsEvaluationById(int vpgsevalId)
        {
            try
            {
                var result = await _vpgsService.GetVpgsEvaluationByIdAsync(vpgsevalId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the VPGS evaluation", details = ex.Message });
            }
        }
        [HttpGet("evaluation/{evaluationId}")]
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
        [HttpGet("period/{periodId}")]
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
        [HttpPut("evaluation/{evaluationId}")]
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
