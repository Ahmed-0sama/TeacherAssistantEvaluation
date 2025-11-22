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
        [HttpGet("GetById{id}")]
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
        [HttpGet("GetByEvaluation/{evaluationId}")]
        public async Task<IActionResult> GetByEvaluationId(int evaluationId)
        {
            try
            {
                var result = await _gsDeanService.GetByEvaluationIdAsync(evaluationId);
                return Ok(result);
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
        [HttpGet("GetEvaluationForTA/{evaluationId}")]
        public async Task<IActionResult> GetEvaluationForTA(int evaluationId)
        {
            try
            {
                var result = await _gsDeanService.GetByEvaluationIdForTAAsync(evaluationId);

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