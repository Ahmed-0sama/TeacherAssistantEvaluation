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
                var result = await _deanService.UpdateEvaluationCriteriaAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
