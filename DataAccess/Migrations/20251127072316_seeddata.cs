using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 5);
        }
    }
}
