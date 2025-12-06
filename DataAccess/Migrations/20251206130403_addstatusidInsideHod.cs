using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addstatusidInsideHod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
