using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editthecoulmnnames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gpa",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                newName: "GPA");

            migrationBuilder.RenameColumn(
                name: "GsdeanEmloyeeId",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                newName: "GSDean_EmloyeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GPA",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                newName: "Gpa");

            migrationBuilder.RenameColumn(
                name: "GSDean_EmloyeeID",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                newName: "GsdeanEmloyeeId");
        }
    }
}
