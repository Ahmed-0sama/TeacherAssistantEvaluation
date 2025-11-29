using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.GSDean;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GSDeanController : ControllerBase
    {
        private readonly IGSDean _gsDeanService;
        public GSDeanController(IGSDean gsDeanService)
        {
            _gsDeanService = gsDeanService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _gsDeanService.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _gsDeanService.GetByIdAsync(id);
                if (result == null) return NotFound("Evaluation not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetByEvaluationPeriod/{evaluationPeriodId}")]
        public async Task<IActionResult> GetByEvaluationPeriodId(int evaluationPeriodId)
        {
            try
            {
                var results = await _gsDeanService.GetByEvaluationPeriodIdAsync(evaluationPeriodId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetByTAEmployee/{taEmployeeId}")]
        public async Task<IActionResult> GetByTAEmployeeId(int taEmployeeId)
        {
            try
            {
                var results = await _gsDeanService.GetByTAEmployeeIdAsync(taEmployeeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("CreateEvaluation")]
        public async Task<IActionResult> Create([FromBody] CreateGsdeanEvaluationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _gsDeanService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.GsevalId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGsdeanEvaluationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _gsDeanService.UpdateAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Evaluation not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetEvaluationForTA/{evaluationPeriodId}/{taEmployeeId}")]
        public async Task<IActionResult> GetEvaluationForTA(int evaluationPeriodId, int taEmployeeId)
        {
            try
            {
                var result = await _gsDeanService.GetByEvaluationPeriodAndTAAsync(evaluationPeriodId, taEmployeeId);
                if (result == null)
                    return NotFound("Evaluation not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}