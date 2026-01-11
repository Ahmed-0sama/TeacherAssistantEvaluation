using Shared.Dtos;
using Shared.Dtos.TASubmissions;

namespace Srs.Client.Services.EvaluationServices
{
    public class TADataMappingService
    {
        public (List<ResearchPaperDto> researchPapers,
            OtherActivitiesDto otherActivities,
            AdminstrativeActivityDto administrativeData,
            StudentActivityDto studentActivityData) MapTASubmissionToComponentData(
        TASubmissionResponseDto existingData)
        {
            var researchPapers = existingData.ResearchActivities?.Select((r, index) => new ResearchPaperDto
            {
                Id = index + 1,
                Title = r.Title,
                ConferenceOrJournal = r.Journal,
                Location = r.Location,
                PageCount = r.PageCount,
                Date = r.ActivityDate,
                Status = r.StatusId,
                Url = r.Url
            }).ToList() ?? new List<ResearchPaperDto>();

            var otherActivities = new OtherActivitiesDto
            {
                HasTechnicalReportsOrArticles = existingData.HasTechnicalReports,
                HasGivenLecturesOrSeminars = existingData.HasSeminarLectures,
                HasAttendedScientificSeminars = existingData.HasAttendingSeminars
            };

            var administrativeData = new AdminstrativeActivityDto
            {
                IsAcademicGuidanceCommittee = existingData.IsInAcademicAdvisingCommittee,
                ISchedulingCommittee = existingData.IsInSchedulingCommittee,
                IsQualityWorksCommittee = existingData.IsInQualityAssuranceCommittee,
                IsLaboratoryEquipmentCommittee = existingData.IsInLabEquipmentCommittee,
                IsExaminationsOrganizingCommittee = existingData.IsInExamOrganizationCommittee,
                IsSocialOrSportsActivityCommittees = existingData.IsInSocialOrSportsCommittee
            };

            var studentActivityData = new StudentActivityDto
            {
                IsCompletedStudentActivity = existingData.ParticipatedInSports,
                IsCompletedSocialActivity = existingData.ParticipatedInSocial,
                IsCompletedCulturalActivity = existingData.ParticipatedInCultural
            };

            return (researchPapers, otherActivities, administrativeData, studentActivityData);
        }

        // Map component data to submission DTO
        public CreateTASubmissionDto MapComponentDataToSubmission(
            EducationalActivityDto educationalData,
            List<ResearchPaperDto> researchPapers,
            OtherActivitiesDto otherActivities,
            AdminstrativeActivityDto administrativeData,
            StudentActivityDto studentActivityData)
        {
            var researchActivities = researchPapers.Select(p => new CreateResearchActivityDto
            {
                Title = p.Title,
                Journal = p.ConferenceOrJournal,
                Location = p.Location,
                PageCount = p.PageCount,
                ActivityDate = p.Date,
                StatusId = p.Status,
                Url = p.Url
            }).ToList();

            return new CreateTASubmissionDto
            {
                ActualTeachingLoad = educationalData.TeachingData.ActualTeachingLoad,
                ExpectedTeachingLoad = educationalData.TeachingData.ExpectedTeachingLoad,
                HasTechnicalReports = otherActivities.HasTechnicalReportsOrArticles,
                HasSeminarLectures = otherActivities.HasGivenLecturesOrSeminars,
                HasAttendingSeminars = otherActivities.HasAttendedScientificSeminars,
                AdvisedStudentCount = 2,
                ResearchActivities = researchActivities,
                IsInAcademicAdvisingCommittee = administrativeData.IsAcademicGuidanceCommittee,
                IsInSchedulingCommittee = administrativeData.ISchedulingCommittee,
                IsInQualityAssuranceCommittee = administrativeData.IsQualityWorksCommittee,
                IsInLabEquipmentCommittee = administrativeData.IsLaboratoryEquipmentCommittee,
                IsInExamOrganizationCommittee = administrativeData.IsExaminationsOrganizingCommittee,
                IsInSocialOrSportsCommittee = administrativeData.IsSocialOrSportsActivityCommittees,
                ParticipatedInSports = studentActivityData.IsCompletedStudentActivity,
                ParticipatedInSocial = studentActivityData.IsCompletedSocialActivity,
                ParticipatedInCultural = studentActivityData.IsCompletedCulturalActivity
            };
        }

        // Map component data to update DTO
        public UpdateTASubmissionsDto MapComponentDataToUpdateDto(
            CreateTASubmissionDto createDto)
        {
            return new UpdateTASubmissionsDto
            {
                ActualTeachingLoad = createDto.ActualTeachingLoad,
                ExpectedTeachingLoad = createDto.ExpectedTeachingLoad,
                HasTechnicalReports = createDto.HasTechnicalReports,
                HasSeminarLectures = createDto.HasSeminarLectures,
                HasAttendingSeminars = createDto.HasAttendingSeminars,
                AdvisedStudentCount = createDto.AdvisedStudentCount,
                ResearchActivities = createDto.ResearchActivities,
                IsInAcademicAdvisingCommittee = createDto.IsInAcademicAdvisingCommittee,
                IsInSchedulingCommittee = createDto.IsInSchedulingCommittee,
                IsInQualityAssuranceCommittee = createDto.IsInQualityAssuranceCommittee,
                IsInLabEquipmentCommittee = createDto.IsInLabEquipmentCommittee,
                IsInExamOrganizationCommittee = createDto.IsInExamOrganizationCommittee,
                IsInSocialOrSportsCommittee = createDto.IsInSocialOrSportsCommittee,
                ParticipatedInSports = createDto.ParticipatedInSports,
                ParticipatedInSocial = createDto.ParticipatedInSocial,
                ParticipatedInCultural = createDto.ParticipatedInCultural
            };
        }
    }
}
