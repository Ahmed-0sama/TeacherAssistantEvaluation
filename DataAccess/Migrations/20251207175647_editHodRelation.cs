using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editHodRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HODEvaluations_StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_HODEvaluations_StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.DropColumn(
                name: "StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                columns: new[] { "CriterionID", "CriterionName", "CriterionType" },
                values: new object[] { 20, "تقييم المشاركة في اللجان", "AdministrativeTotal" });

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "Ratings",
                columns: new[] { "RatingID", "RatingName", "ScoreValue" },
                values: new object[,]
                {
                    { 6, "0", 0 },
                    { 7, "1", 1 },
                    { 8, "2", 2 },
                    { 9, "3", 3 },
                    { 10, "4", 4 },
                    { 11, "5", 5 },
                    { 12, "6", 6 },
                    { 13, "7", 7 },
                    { 14, "8", 8 },
                    { 15, "9", 9 },
                    { 16, "10", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "HODEvaluation_Criteria",
                keyColumn: "CriterionID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 16);

            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HODEvaluations_StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_HODEvaluations_StatusID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                column: "StatusID",
                principalSchema: "taEvaluation",
                principalTable: "EvaluationStatuses",
                principalColumn: "StatusID");
        }
    }
}
