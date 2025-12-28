using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class edithod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_HODEval",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.AlterColumn<string>(
                name: "SourceRole",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "HOD",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_HODEvaluations_EvaluationID",
                schema: "taEvaluation",
                table: "HODEvaluations",
                column: "EvaluationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HODEvaluations_EvaluationID",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.AlterColumn<string>(
                name: "SourceRole",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "HOD");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "UQ_HODEval",
                schema: "taEvaluation",
                table: "HODEvaluations",
                columns: new[] { "EvaluationID", "CriterionID" },
                unique: true);
        }
    }
}
