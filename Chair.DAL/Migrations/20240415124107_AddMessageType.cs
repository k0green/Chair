using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinioFiles_ExecutorServices_ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.DropIndex(
                name: "IX_MinioFiles_ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.DropColumn(
                name: "ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Messages",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "ExecutorServiceId",
                table: "MinioFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinioFiles_ExecutorServiceId",
                table: "MinioFiles",
                column: "ExecutorServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinioFiles_ExecutorServices_ExecutorServiceId",
                table: "MinioFiles",
                column: "ExecutorServiceId",
                principalTable: "ExecutorServices",
                principalColumn: "Id");
        }
    }
}
