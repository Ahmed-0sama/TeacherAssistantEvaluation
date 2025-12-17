using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editdataseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 1,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "TA_ضعيف", 0 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 2,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "TA_مقبول", 1 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 3,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "TA_جيد", 2 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 4,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "TA_جيد جداً", 3 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 5,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "TA_ممتاز", 4 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 6,
                column: "RatingName",
                value: "SA_ضعيف");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 7,
                column: "RatingName",
                value: "SA_مقبول");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 8,
                column: "RatingName",
                value: "SA_جيد");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 9,
                column: "RatingName",
                value: "SA_جيد جداً");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 10,
                column: "RatingName",
                value: "SA_ممتاز");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 11,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "PT_ضعيف", 0 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 12,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "PT_مقبول", 1 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 13,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "PT_جيد", 2 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 14,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "PT_جيد جداً", 3 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 15,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "PT_ممتاز", 4 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 16,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "Admin_0", 0 });

            migrationBuilder.InsertData(
                schema: "taEvaluation",
                table: "Ratings",
                columns: new[] { "RatingID", "RatingName", "ScoreValue" },
                values: new object[,]
                {
                    { 17, "Admin_1", 1 },
                    { 18, "Admin_2", 2 },
                    { 19, "Admin_3", 3 },
                    { 20, "Admin_4", 4 },
                    { 21, "Admin_5", 5 },
                    { 22, "Admin_6", 6 },
                    { 23, "Admin_7", 7 },
                    { 24, "Admin_8", 8 },
                    { 25, "Admin_9", 9 },
                    { 26, "Admin_10", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 26);

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 1,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "ممتاز", 5 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 2,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "جيد جداً", 4 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 3,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "جيد", 3 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 4,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "مقبول", 2 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 5,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "ضعيف", 1 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 6,
                column: "RatingName",
                value: "0");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 7,
                column: "RatingName",
                value: "1");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 8,
                column: "RatingName",
                value: "2");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 9,
                column: "RatingName",
                value: "3");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 10,
                column: "RatingName",
                value: "4");

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 11,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "5", 5 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 12,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "6", 6 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 13,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "7", 7 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 14,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "8", 8 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 15,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "9", 9 });

            migrationBuilder.UpdateData(
                schema: "taEvaluation",
                table: "Ratings",
                keyColumn: "RatingID",
                keyValue: 16,
                columns: new[] { "RatingName", "ScoreValue" },
                values: new object[] { "10", 10 });
        }
    }
}
