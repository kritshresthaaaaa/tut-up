using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyGradeStreamStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "SchoolOrInstitution",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Stream",
                table: "Student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Student",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolOrInstitution",
                table: "Student",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stream",
                table: "Student",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
