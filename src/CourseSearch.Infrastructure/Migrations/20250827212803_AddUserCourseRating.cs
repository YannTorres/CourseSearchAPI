using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCourseRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Ratings_RatingId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_RatingId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "UserCourseRating",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourseRating", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_UserCourseRating_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourseRating_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CourseId",
                table: "Ratings",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCourseRating_CourseId",
                table: "UserCourseRating",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Courses_CourseId",
                table: "Ratings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Courses_CourseId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "UserCourseRating");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CourseId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_RatingId",
                table: "Courses",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Ratings_RatingId",
                table: "Courses",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id");
        }
    }
}
