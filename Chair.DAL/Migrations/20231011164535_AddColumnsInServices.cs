using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsInServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ExecutorServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Duration",
                table: "ExecutorServices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ExecutorServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ExecutorServices");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ExecutorServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ExecutorServices");
        }
    }
}
