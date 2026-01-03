using Shared.Dtos.DeanDto;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IDean
    {
        Task<DeanEvaluationDetailDto?> GetEvaluationDetailAsync(int evaluationId);

        Task<DeanActionResponseDto> AcceptEvaluationAsync(AcceptEvaluationDto dto);

        Task<DeanActionResponseDto> ReturnEvaluationAsync(ReturnEvaluationDto dto);
        Task<DeanActionResponseDto> UpdateEvaluationCriteriaAsync(UpdateDeanEvaluationDto dto);
        Task<List<EvaluationApiResponseDto>> GetTAsForDeanAsync(int periodId, int deanDepartmentId, DateOnly startDate);


    }
}
