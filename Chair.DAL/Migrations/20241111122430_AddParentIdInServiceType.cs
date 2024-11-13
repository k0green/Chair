using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chair.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIdInServiceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ServiceTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_ParentId",
                table: "ServiceTypes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTypes_ServiceTypes_ParentId",
                table: "ServiceTypes",
                column: "ParentId",
                principalTable: "ServiceTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTypes_ServiceTypes_ParentId",
                table: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTypes_ParentId",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ServiceTypes");
        }
    }
}
