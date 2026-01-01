using Shared.Dtos;
using Shared.Dtos.HODEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IHODEvaluation
    {
        Task<int> CreateHodEvaluationAsync(CreateHodEvaluationDto dto);
        Task<bool> UpdateHodEvaluationAsync(int evaluationId, UpdateHodEvaluationDto dto);
        Task<HodEvaluationResponseDto> GetHodEvaluationAsync(int evaluationId);
        Task<List<HodEvaluationResponseDto>> GetHodEvaluationsByPeriodAsync(int periodId);
        Task<bool> HasHodEvaluationAsync(int evaluationId);
        Task<bool> ReturnToTaAsync(ReturnEvaluationHODDto dto);
        Task<bool> ReturnToProfessorAsync(ReturnToProfessor dto);

        Task<List<UserDataDto>> GetTAsForHODAsync(int periodId, int hodDepartmentId, DateOnly startDate);


    }
}
