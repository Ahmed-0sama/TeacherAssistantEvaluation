using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SourceRole",
                schema: "taEvaluation",
                table: "HODEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "taEvaluation",
                table: "HODEvaluations");

            migrationBuilder.DropColumn(
                name: "SourceRole",
                schema: "taEvaluation",
                table: "HODEvaluations");
        }
    }
}
