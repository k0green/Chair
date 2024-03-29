using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixImagesFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ExecutorServices_ObjectId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ObjectId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "ExecutorProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "ExecutorServiceId",
                table: "MinioFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExecutorServiceId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "ExecutorProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinioFiles_ExecutorServiceId",
                table: "MinioFiles",
                column: "ExecutorServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ExecutorServiceId",
                table: "Images",
                column: "ExecutorServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutorProfiles_ImageId",
                table: "ExecutorProfiles",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorProfiles_MinioFiles_ImageId",
                table: "ExecutorProfiles",
                column: "ImageId",
                principalTable: "MinioFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ExecutorServices_ExecutorServiceId",
                table: "Images",
                column: "ExecutorServiceId",
                principalTable: "ExecutorServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MinioFiles_ExecutorServices_ExecutorServiceId",
                table: "MinioFiles",
                column: "ExecutorServiceId",
                principalTable: "ExecutorServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorProfiles_MinioFiles_ImageId",
                table: "ExecutorProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ExecutorServices_ExecutorServiceId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_MinioFiles_ExecutorServices_ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.DropIndex(
                name: "IX_MinioFiles_ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.DropIndex(
                name: "IX_Images_ExecutorServiceId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_ExecutorProfiles_ImageId",
                table: "ExecutorProfiles");

            migrationBuilder.DropColumn(
                name: "ExecutorServiceId",
                table: "MinioFiles");

            migrationBuilder.DropColumn(
                name: "ExecutorServiceId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ExecutorProfiles");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "ExecutorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ObjectId",
                table: "Images",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ExecutorServices_ObjectId",
                table: "Images",
                column: "ObjectId",
                principalTable: "ExecutorServices",
                principalColumn: "Id");
        }
    }
}
