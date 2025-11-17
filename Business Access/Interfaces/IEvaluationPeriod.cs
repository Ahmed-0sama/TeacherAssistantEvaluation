using Shared.Dtos;
using Shared.Dtos.EvaluationPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IEvaluationPeriod
    {
            Task<bool> IsCurrentEvaluationPeriodActiveAsync();
            Task<int?> GetCurrentEvaluationPeriodIdAsync();

            Task<GetEvaluationPeriodDto> GetCurrentEvaluationPeriodAsync();
            Task<int> CreateEvaluationPeriodAsync(CreateEvaluationPeriodDto periodDto);

            Task<GetEvaluationPeriodDto> GetEvaluationPeriodByIdAsync(int periodId);
            Task<IEnumerable<GetEvaluationPeriodDto>> GetAllEvaluationPeriodsAsync();

            Task UpdateEvaluationPeriodAsync(int periodId, CreateEvaluationPeriodDto periodDto);
            Task CloseEvaluationPeriodAsync(int periodId);

    }
}
