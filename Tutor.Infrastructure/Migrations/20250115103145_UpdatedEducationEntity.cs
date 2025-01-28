using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEducationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Education""
                ALTER COLUMN ""Degree"" TYPE integer
                USING CASE ""Degree""
                    WHEN 'Schooling' THEN 1
                    WHEN 'HighSchooling' THEN 2
                    WHEN 'UnderGraduate' THEN 3
                    WHEN 'PostGraduate' THEN 4
                    WHEN 'Doctorate' THEN 5
                    ELSE 1 -- Default to 'Schooling' if unknown value exists
                END;
            ");
            migrationBuilder.AlterColumn<int>(
                name: "Degree",
                table: "Education",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Education""
                ALTER COLUMN ""Degree"" TYPE text
                USING CASE ""Degree""
                    WHEN 1 THEN 'Schooling'
                    WHEN 2 THEN 'HighSchooling'
                    WHEN 3 THEN 'UnderGraduate'
                    WHEN 4 THEN 'PostGraduate'
                    WHEN 5 THEN 'Doctorate'
                    ELSE 'Schooling' -- Default to 'Schooling' if unknown value exists
                END;
            ");
            migrationBuilder.AlterColumn<string>(
                name: "Degree",
                table: "Education",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
