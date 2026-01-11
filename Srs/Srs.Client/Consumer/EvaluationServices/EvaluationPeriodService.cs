using Shared.Dtos.EvaluationPeriod;
using System.Net.Http.Json;

namespace Srs.Client.Services.EvaluationServices
{
    public class EvaluationPeriodService
    {
        private readonly HttpClient _http;

        public EvaluationPeriodService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(bool success, int periodId, string errorMessage)> GetActiveEvaluationPeriodAsync()
        {
            try
            {
                var response = await _http.GetAsync("api/EvaluationPeriod/GetActiveEvaluationPeriod");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetEvaluationPeriodDto>();
                    return (true, result.PeriodId, null);
                }
                else
                {
                    return (false, 0, "لا توجد فترة تقييم نشطة حالياً");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active period: {ex.Message}");
                return (false, 0, "لا توجد فترة تقييم نشطة حالياً");
            }
        }
    }
}
