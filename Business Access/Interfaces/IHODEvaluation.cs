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
        Task UpdateHodEvaluationAsync(int evaluationId, UpdateHodEvaluationDto dto);
        Task<HodEvaluationResponseDto?> GetHodEvaluationByEvaluationIdAsync(int evaluationId);
        Task<List<HodEvaluationResponseDto>> GetHodEvaluationsByPeriodAsync(int periodId);
    }
}
