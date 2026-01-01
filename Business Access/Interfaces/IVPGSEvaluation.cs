using Shared.Dtos;
using Shared.Dtos.VPGSEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IVPGSEvaluation
    {
        Task<int> CreateVpgsEvaluationAsync(CreateVpgsEvaluationDto evaluationDto);

        Task<VpgsEvaluationResponseDto> GetVpgsEvaluationByIdAsync(int vpgsevalId);

        Task<VpgsEvaluationResponseDto?> GetVpgsEvaluationByEvaluationIdAsync(int evaluationId);

        Task<IEnumerable<VpgsEvaluationResponseDto>> GetVpgsEvaluationsByPeriodAsync(int periodId);

        Task UpdateVpgsEvaluationAsync(int evaluationId, UpdateVpgsEvaluationDto evaluationDto);
        Task<List<UserDataDto>> GetGTAsForVPGSAsync(int periodId, int supervisorId, DateOnly startDate);
    }
}
