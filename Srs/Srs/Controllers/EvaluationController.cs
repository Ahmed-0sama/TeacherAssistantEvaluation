using Business_Access.Interfaces;
using Business_Access.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
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
        [HttpPost("getorcreate")]
        public async Task<IActionResult> GetOrCreateEvaluation(int taEmployeeId, int periodId)
        {
            try
            {
                var evaluation = await _evaluationService.GetOrCreateEvaluationAsync(taEmployeeId, periodId);
                return Ok(evaluation);
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
        [HttpPut("UpdateTASubmission/{evaluationid}")]
        public async Task<IActionResult> UpdateTASubmission(int evaluationid, [FromBody] UpdateTASubmissionsDto submissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (evaluationid <= 0)
                    return BadRequest("Invalid submission ID");

                await _evaluationService.UpdateTASubmissionAsync(evaluationid, submissionDto);
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
        [HttpPut("Submit/{Evaluationid}")]
        public async Task<IActionResult> SubmitEvaluation(int Evaluationid,[FromBody] UpdateTASubmissionsDto evaluation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var submittedEvaluation = await _evaluationService.SubmitEvaluation(Evaluationid, evaluation);
                return Ok(submittedEvaluation);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while submitting the evaluation.", details = ex.Message });
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
        [HttpGet("GetGTAInfoWithEvaluation")]
        public async Task<IActionResult> GetGTAInfoWithEvaluation(
            [FromQuery] int taEmployeeId,
            [FromQuery] int periodId)
        {
            try
            {
                var result = await _evaluationService.GetGTAInfoWithEvaluationAsync(taEmployeeId, periodId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetActivityData")]
        public async Task<IActionResult> GetActivityDataForGta(
            [FromQuery] int taEmployeeId,
            [FromQuery] int periodId,
            [FromQuery]int evaluationid,
            [FromQuery]
            DateOnly startDate, 
            [FromQuery]
            DateOnly endDate
            )
        {
            try
            {
                var result = await _evaluationService.GetActivityDataAsync(evaluationid, taEmployeeId, startDate,endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("info/{employeeId}")]
        public async Task<ActionResult<UserDataDto>> GetEmployeeInfo(int employeeId)
        {
            try
            {
                var employeeInfo = await _evaluationService.GetEmployeeInfoAsync(employeeId);

                if (employeeInfo == null)
                {
                    return NotFound($"Employee with ID {employeeId} not found");
                }

                return Ok(employeeInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching employee info: {ex.Message}");
            }
        }

        [HttpGet("teachingData/{employeeId}")]
        public async Task<ActionResult<List<TeachingDataDto>>> GetTeachingData(
            int employeeId,
            [FromQuery] string startDate,
            [FromQuery] string endDate)
        {
            try
            {
                if (!DateOnly.TryParse(startDate, out var start) || !DateOnly.TryParse(endDate, out var end))
                {
                    return BadRequest("Invalid date format. Use yyyy-MM-dd");
                }

                var teachingData = await _evaluationService.GetTeachingDataAsync(employeeId, start, end);
                return Ok(teachingData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching teaching data: {ex.Message}");
            }
        }
    }
}
