using Business_Access.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.TASubmissions;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluation _evaluationService;
        public EvaluationController(IEvaluation evaluationService)
        {
            _evaluationService = evaluationService;
        }
        [HttpPost("CreateEvaluation")]
        public async Task<IActionResult> CreateEvaluation(int TaEmployeeId,int PeriodId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var evaluationId = await _evaluationService.CreateEvaluationAsync(TaEmployeeId, PeriodId);
                return Ok(new { id = evaluationId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the evaluation.", details = ex.Message });
            }
        }
        [HttpPost("{evaluationId}/SubmitTAFiles")]
        public async Task<IActionResult> SubmitTAFiles(int evaluationId, [FromBody] CreateTASubmissionDto submissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (evaluationId <= 0)
                    return BadRequest("Invalid evaluation ID");

                var submissionId = await _evaluationService.SubmitTAFilesAsync(evaluationId, submissionDto);
                return Ok(new { id = submissionId }); ;
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
                return StatusCode(500, new { message = "An error occurred while submitting TA files.", details = ex.Message });
            }
        }
        [HttpPut("UpdateTASubmission/{submissionId}")]
        public async Task<IActionResult> UpdateTASubmission(int submissionId, [FromBody] UpdateTASubmissionsDto submissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (submissionId <= 0)
                    return BadRequest("Invalid submission ID");

                await _evaluationService.UpdateTASubmissionAsync(submissionId, submissionDto);
                return Ok(new { message = "Data Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the submission.", details = ex.Message });
            }
        }
        [HttpGet("GetTASubmission/{evaluationId}")]
        public async Task<IActionResult> GetTASubmission(int evaluationId)
        {
            try
            {
                if (evaluationId <= 0)
                    return BadRequest("Invalid evaluation ID");

                var submission = await _evaluationService.GetTASubmissionAsync(evaluationId);
                return Ok(submission);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the submission.", details = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvaluationById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid evaluation ID");

                var evaluation = await _evaluationService.GetEvaluationByIdAsync(id);
                return Ok(evaluation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the evaluation.", details = ex.Message });
            }
        }
        [HttpGet("CanEdit/{taEmployeeId}")]

        public async Task<IActionResult> CanTAEditEvaluation(int taEmployeeId)
        {
            try
            {
                if (taEmployeeId <= 0)
                    return BadRequest("Invalid TA employee ID");

                var evaluationId = await _evaluationService.CanTAEditEvaluationAsync(taEmployeeId);

                return Ok(new { evaluationId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking edit permissions.", details = ex.Message });
            }
        }
        [HttpGet("period/{periodId}")]
        public async Task<IActionResult> GetEvaluationsByPeriod(int periodId)
        {
            try
            {
                if (periodId <= 0)
                    return BadRequest("Invalid period ID");

                var evaluations = await _evaluationService.GetEvaluationsByPeriodAsync(periodId);
                return Ok(evaluations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving evaluations for the period.", details = ex.Message });
            }
        }
        [HttpGet("ta/{taEmployeeId}/current")]
        public async Task<IActionResult> GetTACurrentEvaluation(int taEmployeeId)
        {
            try
            {
                if (taEmployeeId <= 0)
                    return BadRequest("Invalid TA employee ID");

                var evaluation = await _evaluationService.GetTAEvaluationForCurrentPeriodAsync(taEmployeeId);

                if (evaluation == null)
                    return NotFound(new { message = "Evaluation Id Cannot be null" });

                return Ok(evaluation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the current evaluation.", details = ex.Message });
            }
        }
        [HttpGet("ta/{taEmployeeId}/all")]
        public async Task<IActionResult> GetEvaluationsForTA(int taEmployeeId)
        {
            try
            {
                if (taEmployeeId <= 0)
                    return BadRequest("Invalid TA employee ID");

                var evaluations = await _evaluationService.GetEvaluationsForTAAsync(taEmployeeId);
                return Ok(evaluations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving evaluations for the TA.", details = ex.Message });
            }
        }
    }
}
