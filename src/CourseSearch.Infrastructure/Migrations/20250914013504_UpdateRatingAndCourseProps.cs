using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingAndCourseProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Units",
                table: "Courses");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "UserCourseRating",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "UserCourseRating");

            migrationBuilder.AddColumn<string>(
                name: "Units",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
