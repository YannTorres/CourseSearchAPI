﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseSearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "InteractionTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_InteractionTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Platforms",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Platforms", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tags",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tags", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Active = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Courses",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CourseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        PlatformId = table.Column<int>(type: "int", nullable: false),
            //        Active = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Courses", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Courses_Platforms_PlatformId",
            //            column: x => x.PlatformId,
            //            principalTable: "Platforms",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Roadmaps",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Active = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Roadmaps", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Roadmaps_Users_CreatorId",
            //            column: x => x.CreatorId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CoursesTags",
            //    columns: table => new
            //    {
            //        CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TagId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CoursesTags", x => new { x.CourseId, x.TagId });
            //        table.ForeignKey(
            //            name: "FK_CoursesTags_Courses_CourseId",
            //            column: x => x.CourseId,
            //            principalTable: "Courses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CoursesTags_Tags_TagId",
            //            column: x => x.TagId,
            //            principalTable: "Tags",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserInteractions",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Rating = table.Column<int>(type: "int", nullable: true),
            //        InteractionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        InteractionTypeId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserInteractions", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_UserInteractions_Courses_CourseId",
            //            column: x => x.CourseId,
            //            principalTable: "Courses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserInteractions_InteractionTypes_InteractionTypeId",
            //            column: x => x.InteractionTypeId,
            //            principalTable: "InteractionTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserInteractions_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RoadmapCourses",
            //    columns: table => new
            //    {
            //        RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StepOrder = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RoadmapCourses", x => new { x.RoadmapId, x.CourseId });
            //        table.ForeignKey(
            //            name: "FK_RoadmapCourses_Courses_CourseId",
            //            column: x => x.CourseId,
            //            principalTable: "Courses",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_RoadmapCourses_Roadmaps_RoadmapId",
            //            column: x => x.RoadmapId,
            //            principalTable: "Roadmaps",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Courses_PlatformId",
            //    table: "Courses",
            //    column: "PlatformId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CoursesTags_TagId",
            //    table: "CoursesTags",
            //    column: "TagId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RoadmapCourses_CourseId",
            //    table: "RoadmapCourses",
            //    column: "CourseId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Roadmaps_CreatorId",
            //    table: "Roadmaps",
            //    column: "CreatorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserInteractions_CourseId",
            //    table: "UserInteractions",
            //    column: "CourseId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserInteractions_InteractionTypeId",
            //    table: "UserInteractions",
            //    column: "InteractionTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserInteractions_UserId",
            //    table: "UserInteractions",
            //    column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursesTags");

            migrationBuilder.DropTable(
                name: "RoadmapCourses");

            migrationBuilder.DropTable(
                name: "UserInteractions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Roadmaps");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "InteractionTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
