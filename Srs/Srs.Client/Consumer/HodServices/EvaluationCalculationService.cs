using Shared.Dtos;
using Shared.Dtos.ProfessorEvaluationDto;

namespace Srs.Client.Services.HodServices
{
    public class EvaluationCalculationService
    {
        public int CalculateAverageScore(List<ProfessorEvaluationResponseDto> evaluations)
        {
            if (evaluations == null || !evaluations.Any())
                return 0;

            var average = evaluations.Average(e => e.TotalScore);
            return (int)Math.Round(average);
        }
        public int CalculateTeachingLoadGrade(int actualTeaching, int expectedTeaching)
        {
            return actualTeaching >= expectedTeaching ? 10 : 0;
        }

        public decimal CalculateAverageTeachingLoadGrade(List<TeachingDataDto> semesters)
        {
            if (!semesters.Any())
                return 0;

            return semesters.Average(s => s.score);
        }

        public decimal CalculateTeachingActivitiesTotal(Dictionary<int, int> ratings)
        {
            // Criteria 1-5, RatingId 1-5 maps to points: 0, 0.5, 1, 1.5, 2
            decimal total = 0;
            foreach (var kvp in ratings)
            {
                int ratingId = kvp.Value;
                int scoreValue = ratingId >= 1 && ratingId <= 5 ? ratingId - 1 : 0;
                total += scoreValue * 0.5m;
            }
            return total;
        }

        public int CalculateAdministrativeTotal(Dictionary<int, int> ratings)
        {
            // Criterion 20, RatingId 16-26 maps to score 0-10
            if (ratings.ContainsKey(20))
            {
                int ratingId = ratings[20];
                return ratingId >= 16 && ratingId <= 26 ? ratingId - 16 : 0;
            }
            return 0;
        }

        public decimal CalculateStudentActivitiesTotal(Dictionary<int, int> ratings)
        {
            // Criteria 12-14, RatingId 6-10 maps to points: 0, 1, 2, 3, 3.33
            decimal total = 0;
            foreach (var kvp in ratings)
            {
                int ratingId = kvp.Value;
                int scoreValue = ratingId >= 6 && ratingId <= 10 ? ratingId - 6 : 0;

                total += scoreValue switch
                {
                    0 => 0m,
                    1 => 1m,
                    2 => 2m,
                    3 => 3m,
                    4 => 3.33m,
                    _ => 0m
                };
            }
            return total;
        }

        public decimal CalculatePersonalTraitsTotal(Dictionary<int, int> ratings)
        {
            // Criteria 15-19, RatingId 11-15 maps to points: 0, 0.5, 1, 1.5, 2
            decimal total = 0;
            foreach (var kvp in ratings)
            {
                int ratingId = kvp.Value;
                int scoreValue = ratingId >= 11 && ratingId <= 15 ? ratingId - 11 : 0;
                total += scoreValue * 0.5m;
            }
            return total;
        }

        // ========================================================================
        // FINAL TOTAL CALCULATION
        // ========================================================================

        public decimal CalculateFinalTotal(
            decimal teachingLoadScore,
            decimal scientificTotal,
            int administrativeTotal,
            decimal studentActivitiesTotal,
            decimal personalTraitsTotal)
        {
            return teachingLoadScore +      // 50 points
                   scientificTotal +        // 10 points
                   administrativeTotal +    // 10 points
                   studentActivitiesTotal + // 10 points
                   4 +                      // Survey (4/5)
                   4 +                      // Academic guidance (4/5)
                   personalTraitsTotal;     // 10 points
        }
    }
}
