using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutorServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutorComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutorApprove = table.Column<bool>(type: "bit", nullable: false),
                    ClientApprove = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_ExecutorServices_ExecutorServiceId",
                        column: x => x.ExecutorServiceId,
                        principalTable: "ExecutorServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ExecutorServiceId",
                table: "Orders",
                column: "ExecutorServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
