using Shared.Dtos.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IHRService
    {
        Task<EvaluationStatisticsDto> GetEvaluationStatisticsAsync(int evaluationPeriod);
    }
}
