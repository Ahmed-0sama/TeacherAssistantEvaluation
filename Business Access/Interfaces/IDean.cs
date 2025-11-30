using Shared.Dtos.DeanDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IDean
    {
        Task<IEnumerable<DeanEvaluationListDto>> GetAllEvaluationsAsync(int? statusId = null,int? periodId = null);
        Task<DeanEvaluationDetailDto?> GetEvaluationDetailAsync(int evaluationId);

        Task<DeanActionResponseDto> AcceptEvaluationAsync(AcceptEvaluationDto dto);

        Task<DeanActionResponseDto> ReturnEvaluationAsync(ReturnEvaluationDto dto);

    }
}
