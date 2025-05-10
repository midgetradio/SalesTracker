using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable CS8981
namespace SalesTracker.Migrations
{
    public partial class etlsaletype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleTypeId",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EditionsETL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    URL = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditionsETL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Editions_SaleTypeId",
                table: "Editions",
                column: "SaleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editions_SaleTypes_SaleTypeId",
                table: "Editions",
                column: "SaleTypeId",
                principalTable: "SaleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editions_SaleTypes_SaleTypeId",
                table: "Editions");

            migrationBuilder.DropTable(
                name: "EditionsETL");

            migrationBuilder.DropTable(
                name: "SaleTypes");

            migrationBuilder.DropIndex(
                name: "IX_Editions_SaleTypeId",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "SaleTypeId",
                table: "Editions");
        }
    }
}
