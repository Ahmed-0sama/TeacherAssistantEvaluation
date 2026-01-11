using Shared.Dtos;
using Shared.Dtos.DeanDto;
using Shared.Dtos.HODEvaluation;
using Shared.Dtos.ProfessorEvaluationDto;
using System.Net.Http.Json;

namespace Srs.Client.Services.HodServices
{
    public class HodEvaluationService
    {
        private readonly HttpClient _http;

        public HodEvaluationService(HttpClient http)
        {
            _http = http;
        }
        public async Task<UserDataDto?> GetEmployeeInfoAsync(int employeeId)
        {
            try
            {
                var response = await _http.GetAsync($"api/evaluation/info/{employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDataDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading employee info: {ex.Message}");
            }
            return null;
        }

        public async Task<List<TeachingDataDto>> GetTeachingDataAsync(int employeeId, string startDate, string endDate)
        {
            try
            {
                var response = await _http.GetAsync(
                    $"api/evaluation/teachingdata/{employeeId}?startDate={startDate}&endDate={endDate}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<TeachingDataDto>>()
                        ?? new List<TeachingDataDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading teaching data: {ex.Message}");
            }
            return new List<TeachingDataDto>();
        }

        public async Task<List<ProfessorEvaluationResponseDto>> GetProfessorEvaluationsAsync(int periodId, int taEmployeeId)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<ProfessorEvaluationResponseDto>>(
                    $"api/ProfessorEvaluation/GetByPeriodAndTA/{periodId}/{taEmployeeId}")
                    ?? new List<ProfessorEvaluationResponseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error loading professor evaluations: {ex.Message}");
                return new List<ProfessorEvaluationResponseDto>();
            }
        }
        public async Task<HodEvaluationResponseDto?> GetHodEvaluationAsync(int evaluationId)
        {
            try
            {
                var response = await _http.GetAsync($"api/HodEvaluation/HodEvaluationByEvaluationId/{evaluationId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<HodEvaluationResponseDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error loading HOD evaluation: {ex.Message}");
            }
            return null;
        }

        public async Task<(bool Success, string Message)> CreateHodEvaluationAsync(CreateHodEvaluationDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/HodEvaluation/create", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "تم حفظ التقييم بنجاح");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Create failed: {error}");
                    return (false, $"فشل حفظ التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error creating evaluation: {ex.Message}");
                return (false, $"حدث خطأ: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateHodEvaluationAsync(int evaluationId, UpdateHodEvaluationDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync(
                    $"api/HodEvaluation/UpdateHodEvaluation/{evaluationId}", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "تم تحديث التقييم بنجاح");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Update failed: {error}");
                    return (false, $"فشل تحديث التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error updating evaluation: {ex.Message}");
                return (false, $"حدث خطأ: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateDeanEvaluationAsync(UpdateDeanEvaluationDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("/api/Dean/updateEvaluationCriteria", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "تم تحديث التقييم من قبل العميد بنجاح");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Dean update failed: {error}");
                    return (false, $"فشل تحديث التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in dean update: {ex.Message}");
                return (false, $"حدث خطأ: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ReturnToTAAsync(ReturnEvaluationHODDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/HodEvaluation/ReturnToTA", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "تم إرجاع التقييم إلى مساعد التدريس بنجاح");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Return to TA failed: {error}");
                    return (false, $"فشل إرجاع التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error returning to TA: {ex.Message}");
                return (false, $"حدث خطأ: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ReturnToProfessorAsync(ReturnToProfessor dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/HodEvaluation/ReturnToProfessor", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "تم إرجاع التقييم إلى أستاذ المقرر بنجاح");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($" Return to professor failed: {error}");
                    return (false, $"فشل إرجاع التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error returning to professor: {ex.Message}");
                return (false, $"حدث خطأ: {ex.Message}");
            }
        }
    }
}
