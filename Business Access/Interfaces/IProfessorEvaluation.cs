using DataAccess.Entities;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Access.Interfaces
{
    public interface IProfessorEvaluation
    {
        Task<int> CreateProfessorEvaluationAsync(CreateProfessorEvaluationDto evaluationDto);

        Task UpdateProfessorEvaluationAsync(int evaluationid, UpdateProfessorEvaluationDto evaluationDto);


        Task<ProfessorEvaluationResponseDto> GetProfessorEvaluationByIdAsync(int profEvalId);

        Task<IEnumerable<ProfessorEvaluationResponseDto>> GetEvaluationsByProfessorAsync(int professorEmployeeId);
        Task<List<ProfessorCourseEvaluation>> GetByEvaluationIdAsync(int evaluationId);


    }
}
