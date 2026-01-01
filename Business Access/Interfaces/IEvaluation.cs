using Shared.Dtos;
using Shared.Dtos.TASubmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IEvaluation
    {
        Task<int> SubmitTAFilesAsync(int evaluationId, CreateTASubmissionDto submissionDto);

        Task UpdateTASubmissionAsync(int evalautionid, UpdateTASubmissionsDto submissionDto);

        Task<TASubmissionResponseDto> GetTASubmissionAsync(int evaluationId);
        Task<GetEvaluationDto> GetEvaluationByIdAsync(int evaluationId);

        Task<GetEvaluationDto?> GetTAEvaluationForCurrentPeriodAsync(int taEmployeeId);

        Task<IEnumerable<GetEvaluationDto>> GetEvaluationsForTAAsync(int taEmployeeId);

        Task<IEnumerable<GetEvaluationDto>> GetEvaluationsByStatusAsync(int statusId);

        Task<IEnumerable<GetEvaluationDto>> GetEvaluationsByPeriodAsync(int periodId);

        Task<GetEvaluationDto> GetOrCreateEvaluationAsync(int taEmployeeId, int periodId);

        Task<GetEvaluationDto> SubmitEvaluation(int evaluationId, UpdateTASubmissionsDto submissionDto);
        Task<UserDataDto> GetGTAInfoWithEvaluationAsync(int taEmployeeId, int periodId);
    }
}
