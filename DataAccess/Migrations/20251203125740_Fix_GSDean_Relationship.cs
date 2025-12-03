using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_GSDean_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__GSDean_E__C8B1293995D5C787",
                schema: "taEvaluation",
                table: "GSDean_Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_GSDean_Evaluations_EvaluationPeriodID",
                schema: "taEvaluation",
                table: "GSDean_Evaluations");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "ProgressScore",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProgramName",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "Gpa",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GSDean_Evaluations",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                column: "GSEvalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GSDean_Evaluations",
                schema: "taEvaluation",
                table: "GSDean_Evaluations");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "ProgressScore",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProgramName",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GSDean_E__C8B1293995D5C787",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                column: "GSEvalID");

            migrationBuilder.CreateIndex(
                name: "IX_GSDean_Evaluations_EvaluationPeriodID",
                schema: "taEvaluation",
                table: "GSDean_Evaluations",
                column: "EvaluationPeriodID",
                unique: true);
        }
    }
}
