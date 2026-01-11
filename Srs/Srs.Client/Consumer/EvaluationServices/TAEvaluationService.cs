using Shared.Dtos;
using Shared.Dtos.TASubmissions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Srs.Client.Services.EvaluationServices
{
    public class TAEvaluationService
    {
        private readonly HttpClient _http;

        public TAEvaluationService(HttpClient http)
        {
            _http = http;
        }

        // Load user data
        public async Task<UserDataDto?> GetGTAInfoWithEvaluationAsync(int taEmployeeId, int periodId)
        {
            try
            {
                var response = await _http.GetAsync(
                    $"api/Evaluation/GetGTAInfoWithEvaluation?taEmployeeId={taEmployeeId}&periodId={periodId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDataDto>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to load user data: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
                return null;
            }
        }

        // Load existing TA submission
        public async Task<TASubmissionResponseDto?> GetTASubmissionAsync(int evaluationId)
        {
            try
            {
                var response = await _http.GetAsync($"/api/Evaluation/GetTASubmission/{evaluationId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TASubmissionResponseDto>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading existing TA data: {ex.Message}");
                return null;
            }
        }

        // Get evaluation details
        public async Task<GetEvaluationDto?> GetEvaluationAsync(int evaluationId)
        {
            try
            {
                var response = await _http.GetAsync($"api/Evaluation/{evaluationId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<GetEvaluationDto>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting evaluation: {ex.Message}");
                return null;
            }
        }

        // Create TA submission
        public async Task<(bool success, int? submissionId, string message)> CreateTASubmissionAsync(
            int evaluationId,
            CreateTASubmissionDto data)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(
                    $"/api/Evaluation/{evaluationId}/SubmitTAFiles", data);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Submission created successfully!");

                    // Try to extract submission ID
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(responseContent))
                        {
                            var result = JsonDocument.Parse(responseContent);
                            var root = result.RootElement;

                            if (root.TryGetProperty("id", out var idElement))
                            {
                                int id = idElement.GetInt32();
                                return (true, id, "تم حفظ البيانات بنجاح!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not extract submission ID: {ex.Message}");
                    }

                    return (true, null, "تم حفظ البيانات بنجاح!");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create submission failed: {error}");
                    return (false, null, $"فشل في حفظ البيانات: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during create: {ex.Message}");
                return (false, null, $"حدث خطأ أثناء الحفظ: {ex.Message}");
            }
        }

        // Update TA submission
        public async Task<(bool success, string message)> UpdateTASubmissionAsync(
            int evaluationId,
            UpdateTASubmissionsDto data)
        {
            try
            {
                Console.WriteLine($"Updating submission for evaluation ID: {evaluationId}");
                var response = await _http.PutAsJsonAsync(
                    $"/api/Evaluation/UpdateTASubmission/{evaluationId}", data);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Submission updated successfully!");
                    return (true, "تم تحديث البيانات بنجاح!");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Update submission failed: {error}");
                    return (false, $"فشل في تحديث البيانات: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during update: {ex.Message}");
                return (false, $"حدث خطأ أثناء التحديث: {ex.Message}");
            }
        }

        // Submit final evaluation
        public async Task<(bool success, string message)> SubmitFinalEvaluationAsync(
            int evaluationId,
            UpdateTASubmissionsDto data)
        {
            try
            {
                var response = await _http.PutAsJsonAsync(
                    $"/api/Evaluation/Submit/{evaluationId}", data);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Evaluation submitted successfully!");
                    return (true, "تم إرسال التقييم بنجاح!");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Submit failed: {error}");
                    return (false, $"فشل في إرسال التقييم: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during submit: {ex.Message}");
                return (false, $"حدث خطأ أثناء الإرسال: {ex.Message}");
            }
        }
    }
}
