using Business_Access.Interfaces;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;
using Shared.Dtos.ProfessorEvaluationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business_Access.Services
{
    public class ExternalApiService: IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ExternalApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ExternalApi:BaseUrl"];
        }
        public async Task<ProfessorDataResponseDto> GetProfessorCoursesAsync(int ProfessorId,DateOnly StartDate, DateOnly EndDate)
        {
            var url = $"{_baseUrl}/TAEvaluation_api/api/lms/professor-data/{ProfessorId}/{StartDate:yyyy-MM-dd}/{EndDate:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ProfessorDataResponseDto>(jsonString);

            return data;
        }
        public async Task<List<UserDataDto>> GetGTAListAsync(int supervisorId, DateOnly startDate)
        {
            var url = $"{_baseUrl}/TAEvaluation_api/api/HR/GTA_List/{supervisorId}/{startDate:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<List<UserDataDto>>(jsonString, options);

            return data ?? new List<UserDataDto>();
        }
        public async Task<UserDataDto?> GetEmployeeInfoAsync(int employeeId)
        {
            var url = $"{_baseUrl}/TAEvaluation_api/api/hr/get-employee-info/{employeeId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<UserDataDto>(jsonString, options);
            UserDataDto name=new UserDataDto();
            name.EmployeeNumber=data.EmployeeNumber;
            name.employeeName=data.employeeName;
            name.employeeId = data.employeeId;
            return data;
        }
    }
}
