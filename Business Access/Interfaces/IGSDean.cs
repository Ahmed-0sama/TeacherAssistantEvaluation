using DataAccess.Entities;
using Shared.Dtos.GSDean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IGSDean
    {
        Task<GsdeanEvaluationDto?> GetByIdAsync(int gsevalId);
        Task<IEnumerable<GsdeanEvaluationDto>> GetAllAsync();
        Task<GsdeanEvaluationDto> CreateAsync(CreateGsdeanEvaluationDto dto);
        Task<GsdeanEvaluationDto> UpdateAsync(UpdateGsdeanEvaluationDto dto);
        Task<GSDeanTAViewDto> GetByEvaluationPeriodAndTAAsync(int evaluationPeriodId, int taEmployeeId);
        Task<IEnumerable<GsdeanEvaluationDto>> GetByEvaluationPeriodIdAsync(int evaluationPeriodId);
        Task<IEnumerable<GsdeanEvaluationDto>> GetByTAEmployeeIdAsync(int taEmployeeId);


    }
}
