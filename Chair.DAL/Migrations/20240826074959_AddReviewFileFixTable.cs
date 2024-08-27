using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewFileFixTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ReviewFiles",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinioFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewFiles", x => new { x.ProductId, x.MinioFileId });
                    table.ForeignKey(
                        name: "FK_ReviewFiles_MinioFiles_MinioFileId",
                        column: x => x.MinioFileId,
                        principalTable: "MinioFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewFiles_Reviews_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewFiles_MinioFileId",
                table: "ReviewFiles",
                column: "MinioFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewFiles");

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
    }
}
