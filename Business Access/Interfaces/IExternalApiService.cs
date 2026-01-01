using Shared.Dtos;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IExternalApiService
    {
        Task<ProfessorDataResponseDto> GetProfessorCoursesAsync(int ProfessorId, DateOnly StartDate, DateOnly EndDate);
        Task<List<UserDataDto>> GetGTAListAsync(int supervisorId, DateOnly startDate);
        Task<UserDataDto?> GetEmployeeInfoAsync(int employeeId);
    }
}
