using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreCoursesProperties2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationsInHours",
                table: "Courses",
                newName: "RatingId");

            migrationBuilder.AddColumn<int>(
                name: "DurationsInMinutes",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Popularity",
                table: "Courses",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Units",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Average = table.Column<float>(type: "real", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_RatingId",
                table: "Courses",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Rating_RatingId",
                table: "Courses",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Rating_RatingId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Courses_RatingId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DurationsInMinutes",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Courses",
                newName: "DurationsInHours");
        }
    }
}
