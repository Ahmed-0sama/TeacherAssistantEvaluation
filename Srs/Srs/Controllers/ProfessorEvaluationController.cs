using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.ProfessorEvaluationDto;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorEvaluationController : ControllerBase
    {
        private readonly IProfessorEvaluation IProfessorEvaluation;
        public ProfessorEvaluationController(IProfessorEvaluation IProfessorEvaluation)
        {
            this.IProfessorEvaluation = IProfessorEvaluation;
        }
        [HttpPost("CreateProfessorEvaluation")]
        public async Task<IActionResult> Create([FromBody] CreateProfessorEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var profEvalId = await IProfessorEvaluation.CreateProfessorEvaluationAsync(dto);
                return Ok(new { ProfEvalId = profEvalId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Get a professor evaluation by its ID
        [HttpGet("GetProfessorEvaluationById{profEvalId}")]
        public async Task<IActionResult> GetProfessorEvaluationByIdAsync(int profEvalId)
        {
            try
            {
                var result = await IProfessorEvaluation.GetProfessorEvaluationByIdAsync(profEvalId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        // Get all evaluations by a professor
        [HttpGet("professor/{professorEmployeeId}")]
        public async Task<IActionResult> GetByProfessor(int professorEmployeeId)
        {
            try
            {
                var results = await IProfessorEvaluation.GetEvaluationsByProfessorAsync(professorEmployeeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("UpdateEvaluationByEvaluationId{evaluationid}")]
        public async Task<IActionResult> Update(int evaluationid, [FromBody] UpdateProfessorEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await IProfessorEvaluation.UpdateProfessorEvaluationAsync(evaluationid, dto);
                return Ok(new { message = "Professor evaluation updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}