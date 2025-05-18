using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesTracker.Migrations
{
    public partial class SaleTypeETLRmvDatesActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "SaleTypesETL");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "SaleTypesETL");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SaleTypesETL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "SaleTypesETL",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "SaleTypesETL",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SaleTypesETL",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
