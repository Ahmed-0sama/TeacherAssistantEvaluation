using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class databaseupdatestrcture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "taEvaluation");

            migrationBuilder.CreateTable(
                name: "EvaluationPeriods",
                schema: "taEvaluation",
                columns: table => new
                {
                    PeriodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluati__E521BB36E10D5CDC", x => x.PeriodID);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationStatuses",
                schema: "taEvaluation",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluati__C8EE20437A4A7778", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "HODEvaluation_Criteria",
                schema: "taEvaluation",
                columns: table => new
                {
                    CriterionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriterionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CriterionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HODEvalu__647C3BD119DF663C", x => x.CriterionID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "taEvaluation",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E3254FA65A4", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                schema: "taEvaluation",
                columns: table => new
                {
                    RatingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScoreValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ratings__FCCDF85C285DEC07", x => x.RatingID);
                });

            migrationBuilder.CreateTable(
                name: "ResearchStatuses",
                schema: "taEvaluation",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusKey = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Research__C8EE20431C47CE6C", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                schema: "taEvaluation",
                columns: table => new
                {
                    EvaluationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TA_EmployeeID = table.Column<int>(type: "int", nullable: false),
                    PeriodID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    HOD_Strengths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HOD_Weaknesses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HOD_ReturnComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dean_ReturnComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentSurveyScore = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateApproved = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluati__36AE68D3DC67C19C", x => x.EvaluationID);
                    table.ForeignKey(
                        name: "FK_Evaluations_PeriodID",
                        column: x => x.PeriodID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationPeriods",
                        principalColumn: "PeriodID");
                    table.ForeignKey(
                        name: "FK_Evaluations_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationStatuses",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "GSDean_Evaluations",
                schema: "taEvaluation",
                columns: table => new
                {
                    GSEvalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationPeriodID = table.Column<int>(type: "int", nullable: false),
                    GSDean_EmloyeeID = table.Column<int>(type: "int", nullable: true),
                    TA_EmployeeID = table.Column<int>(type: "int", nullable: true),
                    ProgramName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CompletedHours = table.Column<int>(type: "int", nullable: false),
                    GPA = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ExpectedCompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProgressScore = table.Column<decimal>(type: "decimal(3,1)", nullable: true),
                    EvaluationComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicChosen = table.Column<bool>(type: "bit", nullable: false),
                    LiteratureReview = table.Column<bool>(type: "bit", nullable: false),
                    ResearchPlan = table.Column<bool>(type: "bit", nullable: false),
                    DataCollection = table.Column<bool>(type: "bit", nullable: false),
                    Writing = table.Column<bool>(type: "bit", nullable: false),
                    ThesisDefense = table.Column<bool>(type: "bit", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GSDean_E__C8B1293995D5C787", x => x.GSEvalID);
                    table.ForeignKey(
                        name: "FK_GSDean_Evaluations_EvaluationPeriodID",
                        column: x => x.EvaluationPeriodID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationPeriods",
                        principalColumn: "PeriodID");
                    table.ForeignKey(
                        name: "FK_GSDean_Evaluations_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationStatuses",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "ProfessorCourseEvaluations",
                schema: "taEvaluation",
                columns: table => new
                {
                    ProfEvalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationPeriodID = table.Column<int>(type: "int", nullable: false),
                    TA_EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Professor_EmployeeID = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OfficeHoursScore = table.Column<int>(type: "int", nullable: false),
                    AttendanceScore = table.Column<int>(type: "int", nullable: false),
                    PerformanceScore = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(([OfficeHoursScore]+[AttendanceScore])+[PerformanceScore])", stored: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    HOD_ReturnComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Professo__DACBBA35DF4A614E", x => x.ProfEvalID);
                    table.ForeignKey(
                        name: "FK_ProfessorCourseEvaluations_EvaluationPeriodID",
                        column: x => x.EvaluationPeriodID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationPeriods",
                        principalColumn: "PeriodID");
                    table.ForeignKey(
                        name: "FK_ProfessorCourseEvaluations_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationStatuses",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "HODEvaluations",
                schema: "taEvaluation",
                columns: table => new
                {
                    HODEvalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationID = table.Column<int>(type: "int", nullable: false),
                    CriterionID = table.Column<int>(type: "int", nullable: false),
                    RatingID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HODEvalu__6A4AF4FAEBE6B771", x => x.HODEvalID);
                    table.ForeignKey(
                        name: "FK_HODEvaluations_CriterionID",
                        column: x => x.CriterionID,
                        principalSchema: "taEvaluation",
                        principalTable: "HODEvaluation_Criteria",
                        principalColumn: "CriterionID");
                    table.ForeignKey(
                        name: "FK_HODEvaluations_EvaluationID",
                        column: x => x.EvaluationID,
                        principalSchema: "taEvaluation",
                        principalTable: "Evaluations",
                        principalColumn: "EvaluationID");
                    table.ForeignKey(
                        name: "FK_HODEvaluations_RatingID",
                        column: x => x.RatingID,
                        principalSchema: "taEvaluation",
                        principalTable: "Ratings",
                        principalColumn: "RatingID");
                });

            migrationBuilder.CreateTable(
                name: "ReminderLogs",
                schema: "taEvaluation",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationID = table.Column<int>(type: "int", nullable: false),
                    SentBy_EmployeeID = table.Column<int>(type: "int", nullable: false),
                    RecievedBy_EmployeeID = table.Column<int>(type: "int", nullable: false),
                    RecipientDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reminder__5E5499A80E5A40CB", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_ReminderLogs_EvaluationID",
                        column: x => x.EvaluationID,
                        principalSchema: "taEvaluation",
                        principalTable: "Evaluations",
                        principalColumn: "EvaluationID");
                });

            migrationBuilder.CreateTable(
                name: "TASubmissions",
                schema: "taEvaluation",
                columns: table => new
                {
                    SubmissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationID = table.Column<int>(type: "int", nullable: false),
                    ActualTeachingLoad = table.Column<int>(type: "int", nullable: false),
                    ExpectedTeachingLoad = table.Column<int>(type: "int", nullable: false),
                    HasTechnicalReports = table.Column<bool>(type: "bit", nullable: false),
                    HasSeminarLectures = table.Column<bool>(type: "bit", nullable: false),
                    HasAttendingSeminars = table.Column<bool>(type: "bit", nullable: false),
                    IsInAcademicAdvisingCommittee = table.Column<bool>(type: "bit", nullable: false),
                    IsInSchedulingCommittee = table.Column<bool>(type: "bit", nullable: false),
                    IsInQualityAssuranceCommittee = table.Column<bool>(type: "bit", nullable: false),
                    IsInLabEquipmentCommittee = table.Column<bool>(type: "bit", nullable: false),
                    IsInExamOrganizationCommittee = table.Column<bool>(type: "bit", nullable: false),
                    IsInSocialOrSportsCommittee = table.Column<bool>(type: "bit", nullable: false),
                    ParticipatedInSports = table.Column<bool>(type: "bit", nullable: false),
                    ParticipatedInSocial = table.Column<bool>(type: "bit", nullable: false),
                    ParticipatedInCultural = table.Column<bool>(type: "bit", nullable: false),
                    AdvisedStudentCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TASubmis__449EE105411A67B0", x => x.SubmissionID);
                    table.ForeignKey(
                        name: "FK_TA_Submissions_EvaluationID",
                        column: x => x.EvaluationID,
                        principalSchema: "taEvaluation",
                        principalTable: "Evaluations",
                        principalColumn: "EvaluationID");
                });

            migrationBuilder.CreateTable(
                name: "VPGS_Evaluations",
                schema: "taEvaluation",
                columns: table => new
                {
                    VPGSEvalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationID = table.Column<int>(type: "int", nullable: false),
                    ScientificScore = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VPGS_Eva__57C1D3BEE96AAFED", x => x.VPGSEvalID);
                    table.ForeignKey(
                        name: "FK_VPGS_Evaluations_Evaluations_EvaluationID",
                        column: x => x.EvaluationID,
                        principalSchema: "taEvaluation",
                        principalTable: "Evaluations",
                        principalColumn: "EvaluationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VPGS_Evaluations_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "taEvaluation",
                        principalTable: "EvaluationStatuses",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "ResearchActivities",
                schema: "taEvaluation",
                columns: table => new
                {
                    ActivityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Journal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    ActivityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Research__45F4A7F1C12D163E", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_ResearchActivities_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "taEvaluation",
                        principalTable: "ResearchStatuses",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_ResearchActivities_SubmissionID",
                        column: x => x.SubmissionID,
                        principalSchema: "taEvaluation",
                        principalTable: "TASubmissions",
                        principalColumn: "SubmissionID");
                });

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                columns: new[] { "CriterionID", "CriterionName", "CriterionType" },
                values: new object[,]
                {
                    { 1, "إعداد مذكرات للجزء العملي/تدريبات", "DirectTeaching" },
                    { 2, "إعداد مساعدات تعليمية وتدريسية جديدة", "DirectTeaching" },
                    { 3, "المساعدة في إعداد التجارب العملية/التمارين", "DirectTeaching" },
                    { 4, "المشاركة في تنظيم وإدارة دورات تدريسية/مؤتمرات", "DirectTeaching" },
                    { 5, "أي نشاط تعليمي آخر مكلف به", "DirectTeaching" },
                    { 6, "لجنة الإرشاد الأكاديمي", "Administrative" },
                    { 7, "لجنة الجدولة", "Administrative" },
                    { 8, "لجنة أعمال الجودة", "Administrative" },
                    { 9, "لجنة التجهيزات المعملية", "Administrative" },
                    { 10, "لجنة تنظيم امتحانات", "Administrative" },
                    { 11, "لجان النشاط الاجتماعي أو الرياضي", "Administrative" },
                    { 12, "نشاط رياضي", "StudentActivities" },
                    { 13, "نشاط اجتماعي", "StudentActivities" },
                    { 14, "نشاط ثقافي", "StudentActivities" },
                    { 15, "التعاون والعمل الجماعي", "PersonalTraits" },
                    { 16, "الالتزام بالمواعيد", "PersonalTraits" },
                    { 17, "المظهر العام", "PersonalTraits" },
                    { 18, "المبادرة وتحمل المسؤولية", "PersonalTraits" },
                    { 19, "إدارة الوقت", "PersonalTraits" }
                });

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "Ratings",
                columns: new[] { "RatingID", "RatingName", "ScoreValue" },
                values: new object[,]
                {
                    { 1, "ممتاز", 5 },
                    { 2, "جيد جداً", 4 },
                    { 3, "جيد", 3 },
                    { 4, "مقبول", 2 },
                    { 5, "ضعيف", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Evaluati__D748F8F21DD95950",
                schema: "taEvaluation",
                table: "EvaluationPeriods",
                column: "PeriodName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_PeriodID",
                schema: "taEvaluation",
                table: "Evaluations",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_StatusID",
                schema: "taEvaluation",
                table: "Evaluations",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "UQ__Evaluati__05E7698A9628677D",
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GSDean_Evaluations_StatusID",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "UQ__GSDean_E__EvaluationPeriodID",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                column: "EvaluationPeriodID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HODEvaluations_CriterionID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                column: "CriterionID");

            migrationBuilder.CreateIndex(
                name: "IX_HODEvaluations_RatingID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                column: "RatingID");

            migrationBuilder.CreateIndex(
                name: "UQ_HODEvaluations",
                schema: "taEvaluation",
                table: "HODEvaluations",
                columns: new[] { "EvaluationID", "CriterionID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorCourseEvaluations_EvaluationPeriodID",
                schema: "taEvaluation",
                table: "ProfessorCourseEvaluations",
                column: "EvaluationPeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorCourseEvaluations_StatusID",
                schema: "taEvaluation",
                table: "ProfessorCourseEvaluations",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "UQ__Ratings__F7CF97379B70F7DF",
                schema: "taEvaluation",
                table: "Ratings",
                column: "RatingName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReminderLogs_EvaluationID",
                schema: "taEvaluation",
                table: "ReminderLogs",
                column: "EvaluationID");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchActivities_StatusID",
                schema: "taEvaluation",
                table: "ResearchActivities",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchActivities_SubmissionID",
                schema: "taEvaluation",
                table: "ResearchActivities",
                column: "SubmissionID");

            migrationBuilder.CreateIndex(
                name: "UQ__Research__096C98C27DE9011A",
                schema: "taEvaluation",
                table: "ResearchStatuses",
                column: "StatusKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__TASubmis__36AE68D20234AA6E",
                schema: "taEvaluation",
                table: "TASubmissions",
                column: "EvaluationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VPGS_Evaluations_StatusID",
                schema: "taEvaluation",
                table: "VPGS_Evaluations",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "UQ__VPGS_Eva__36AE68D2910EDF18",
                schema: "taEvaluation",
                table: "VPGS_Evaluations",
                column: "EvaluationID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GSDean_Evaluations",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "HODEvaluations",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "ProfessorCourseEvaluations",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "ReminderLogs",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "ResearchActivities",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "VPGS_Evaluations",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "HODEvaluation_Criteria",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "Ratings",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "ResearchStatuses",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "TASubmissions",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "Evaluations",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "EvaluationPeriods",
                schema: "taEvaluation");

            migrationBuilder.DropTable(
                name: "EvaluationStatuses",
                schema: "taEvaluation");
        }
    }
}
