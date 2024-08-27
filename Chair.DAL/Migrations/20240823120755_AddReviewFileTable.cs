using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "ExecutorServiceFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutorServiceFiles_ReviewId",
                table: "ExecutorServiceFiles",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorServiceFiles_Reviews_ReviewId",
                table: "ExecutorServiceFiles",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorServiceFiles_Reviews_ReviewId",
                table: "ExecutorServiceFiles");

            migrationBuilder.DropIndex(
                name: "IX_ExecutorServiceFiles_ReviewId",
                table: "ExecutorServiceFiles");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "ExecutorServiceFiles");
        }
    }
}
