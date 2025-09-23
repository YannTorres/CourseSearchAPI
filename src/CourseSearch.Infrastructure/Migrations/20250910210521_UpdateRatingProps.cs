using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "UserCourseRating",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "UserCourseRating");
        }
    }
}
