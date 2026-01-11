using Shared.Dtos;
using Shared.Dtos.ProfessorEvaluationDto;

namespace Srs.Client.Services.HodServices
{
    public class ProfessorReminderService
    {
        public List<ProfessorReminderInfo> GetProfessorsNeedingReminder(
            List<TeachingDataDto> semesterTeachingLoads,
            List<ProfessorEvaluationResponseDto> submittedEvaluations)
        {
            try
            {
                // Get all professor-course-semester combinations
                var allCombos = semesterTeachingLoads
                    .SelectMany(semester => (semester.courses ?? new List<CourseDto>())
                        .Where(course => !string.IsNullOrEmpty(course.courseProfessorId) &&
                                        !string.IsNullOrEmpty(course.courseCode))
                        .Select(course => new
                        {
                            ProfessorId = int.Parse(course.courseProfessorId.Trim()),
                            ProfessorName = course.courseProfessorName?.Trim(),
                            CourseCode = course.courseCode.Trim(),
                            CourseName = course.courseName?.Trim(),
                            SemesterCode = semester.semestercode,
                            SemesterName = semester.semesterName ?? "Unknown"
                        }))
                    .Distinct()
                    .ToList();

                Console.WriteLine($"Found {allCombos.Count} professor-course-semester assignments");

                // Get submitted combinations
                var submittedCombos = (submittedEvaluations ?? new List<ProfessorEvaluationResponseDto>())
                    .Where(p => p.IsSubmitted)
                    .Select(p => new
                    {
                        p.ProfessorEmployeeId,
                        p.CourseCode,
                        p.SemesterCode
                    })
                    .ToList();

                Console.WriteLine($"Found {submittedCombos.Count} submitted combinations");

                // Find non-submitted
                var needingReminder = allCombos
                    .Where(combo => !submittedCombos.Any(sc =>
                        sc.ProfessorEmployeeId == combo.ProfessorId &&
                        sc.CourseCode == combo.CourseCode &&
                        sc.SemesterCode == combo.SemesterCode))
                    .Select(combo => new ProfessorReminderInfo
                    {
                        ProfessorId = combo.ProfessorId,
                        ProfessorName = combo.ProfessorName,
                        CourseCode = combo.CourseCode,
                        CourseName = combo.CourseName,
                        SemesterCode = combo.SemesterCode,
                        SemesterName = combo.SemesterName
                    })
                    .ToList();

                Console.WriteLine($"Found {needingReminder.Count} professor-course-semesters needing reminder");

                return needingReminder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error in GetProfessorsNeedingReminder: {ex.Message}");
                return new List<ProfessorReminderInfo>();
            }
        }

        public bool AllProfessorsHaveSubmitted(
            List<TeachingDataDto> semesterTeachingLoads,
            List<ProfessorEvaluationResponseDto> submittedEvaluations)
        {
            if (!semesterTeachingLoads.Any())
            {
                Console.WriteLine("No teaching loads found");
                return true;
            }

            // Get all required combinations
            var allCombos = semesterTeachingLoads
                .SelectMany(semester => (semester.courses ?? new List<CourseDto>())
                    .Where(course => !string.IsNullOrEmpty(course.courseProfessorId) &&
                                    !string.IsNullOrEmpty(course.courseCode))
                    .Select(course => new
                    {
                        ProfessorId = int.Parse(course.courseProfessorId.Trim()),
                        CourseCode = course.courseCode.Trim(),
                        SemesterCode = semester.semestercode
                    }))
                .Distinct()
                .ToList();

            if (!allCombos.Any())
            {
                Console.WriteLine("No professors found in teaching data");
                return true;
            }

            // Get submitted combinations
            var submittedCombos = (submittedEvaluations ?? new List<ProfessorEvaluationResponseDto>())
                .Where(p => p.IsSubmitted)
                .Select(p => new
                {
                    ProfessorId = p.ProfessorEmployeeId,
                    p.CourseCode,
                    SemesterCode = p.SemesterCode
                })
                .Distinct()
                .ToList();

            bool allSubmitted = allCombos.All(combo =>
                submittedCombos.Any(submitted =>
                    submitted.ProfessorId == combo.ProfessorId &&
                    submitted.CourseCode == combo.CourseCode &&
                    submitted.SemesterCode == combo.SemesterCode));

            Console.WriteLine($"Professor-Course-Semester check:");
            Console.WriteLine($"   Total required: {allCombos.Count}");
            Console.WriteLine($"   Submitted: {submittedCombos.Count}");
            Console.WriteLine($"   All submitted: {allSubmitted}");

            return allSubmitted;
        }
    }
}