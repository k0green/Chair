using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExecutorPromotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutorPromotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutorPromotion_ExecutorProfiles_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "ExecutorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFile<ExecutorPromotion>",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinioFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFile<ExecutorPromotion>", x => new { x.ProductId, x.MinioFileId });
                    table.ForeignKey(
                        name: "FK_ProductFile<ExecutorPromotion>_ExecutorPromotion_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ExecutorPromotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFile<ExecutorPromotion>_MinioFiles_MinioFileId",
                        column: x => x.MinioFileId,
                        principalTable: "MinioFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutorPromotion_ExecutorId",
                table: "ExecutorPromotion",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFile<ExecutorPromotion>_MinioFileId",
                table: "ProductFile<ExecutorPromotion>",
                column: "MinioFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFile<ExecutorPromotion>");

            migrationBuilder.DropTable(
                name: "ExecutorPromotion");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Orders");
        }
    }
}
