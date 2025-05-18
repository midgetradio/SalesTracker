using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Migrations
{
    public partial class SaleTypeETL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "SaleTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "SaleTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SaleTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ApiModel",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SaleTypesETL",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiModel");

            migrationBuilder.DropTable(
                name: "SaleTypesETL");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "SaleTypes");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "SaleTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SaleTypes");
        }
    }
}
