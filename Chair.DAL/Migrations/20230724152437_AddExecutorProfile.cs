using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddExecutorProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorServices_AspNetUsers_ExecutorId",
                table: "ExecutorServices");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExecutorId",
                table: "ExecutorServices",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ExecutorProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutorProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutorProfiles_UserId",
                table: "ExecutorProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorServices_ExecutorProfiles_ExecutorId",
                table: "ExecutorServices",
                column: "ExecutorId",
                principalTable: "ExecutorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorServices_ExecutorProfiles_ExecutorId",
                table: "ExecutorServices");

            migrationBuilder.DropTable(
                name: "ExecutorProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "ExecutorId",
                table: "ExecutorServices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorServices_AspNetUsers_ExecutorId",
                table: "ExecutorServices",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
