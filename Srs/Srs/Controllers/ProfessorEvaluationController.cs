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
        private readonly IProfessorEvaluation _professorEvaluation;
        public ProfessorEvaluationController(IProfessorEvaluation professorEvaluation)
        {
            _professorEvaluation = professorEvaluation;
        }
        [HttpPost("CreateProfessorEvaluation")]
        public async Task<IActionResult> Create([FromBody] CreateProfessorEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var profEvalId = await _professorEvaluation.CreateProfessorEvaluationAsync(dto);
                return Ok(new { ProfEvalId = profEvalId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Get a professor evaluation by its ID
        [HttpGet("GetProfessorEvaluationById/{profEvalId}")]
        public async Task<IActionResult> GetProfessorEvaluationByIdAsync(int profEvalId)
        {
            try
            {
                var result = await _professorEvaluation.GetProfessorEvaluationByIdAsync(profEvalId);
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
        [HttpGet("GetByEvaluationPeriodId/{evaluationPeriodId}")]
        public async Task<IActionResult> GetByEvaluationPeriodIdAsync(int evaluationPeriodId)
        {
            try
            {
                var results = await _professorEvaluation.GetByEvaluationIdAsync(evaluationPeriodId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetByProfessor/{professorEmployeeId}")]  // CLEANED UP: Better naming
        public async Task<IActionResult> GetByProfessor(int professorEmployeeId)
        {
            try
            {
                var results = await _professorEvaluation.GetEvaluationsByProfessorAsync(professorEmployeeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetByTAEmployee/{taEmployeeId}")]
        public async Task<IActionResult> GetByTAEmployeeId(int taEmployeeId)
        {
            try
            {
                var results = await _professorEvaluation.GetByTAEmployeeIdAsync(taEmployeeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetByPeriodAndTA/{evaluationPeriodId}/{taEmployeeId}")]
        public async Task<IActionResult> GetByPeriodAndTA(int evaluationPeriodId, int taEmployeeId)
        {
            try
            {
                var results = await _professorEvaluation.GetByPeriodAndTAAsync(evaluationPeriodId, taEmployeeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("UpdateEvaluation/{profEvalId}")]
        public async Task<IActionResult> Update(int profEvalId, [FromBody] UpdateProfessorEvaluationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _professorEvaluation.UpdateProfessorEvaluationAsync(profEvalId, dto);
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
        [HttpGet("GetCoursesWithEvaluations")]
        public async Task<IActionResult> GetCoursesWithEvaluations(
            [FromQuery] int professorId,
            [FromQuery] int evaluationPeriodId,
            [FromQuery] DateOnly StartDate,
            [FromQuery] DateOnly EndDate)
        {
            try
            {
                var result = await _professorEvaluation.GetProfessorCoursesWithEvaluationsAsync(
                    professorId,
                    evaluationPeriodId,
                    StartDate,
                    EndDate
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}