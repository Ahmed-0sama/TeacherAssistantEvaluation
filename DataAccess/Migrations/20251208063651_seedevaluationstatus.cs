using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedevaluationstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                columns: new[] { "StatusID", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 1, "NUTA has not submitted their evaluation yet", "Draft" },
                    { 2, "Evaluation submitted by TA and awaiting HOD review", "Submitted" },
                    { 3, "HOD has reviewed and provided comments", "ReviewedByHOD" },
                    { 4, "HOD returned the evaluation to the TA for corrections", "ReturnedByHOD" },
                    { 5, "Dean has reviewed the evaluation", "ReviewedByDean" },
                    { 6, "Dean returned the evaluation for corrections", "ReturnedByDean" },
                    { 7, "Evaluation fully approved and completed", "Approved" }
                });

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "ResearchStatuses",
                columns: new[] { "StatusID", "StatusKey", "StatusName" },
                values: new object[,]
                {
                    { 1, "P", "Published" },
                    { 2, "S", "Submitted" },
                    { 3, "R", "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "EvaluationStatuses",
                keyColumn: "StatusID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "ResearchStatuses",
                keyColumn: "StatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "ResearchStatuses",
                keyColumn: "StatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "ResearchStatuses",
                keyColumn: "StatusID",
                keyValue: 3);
        }
    }
}
