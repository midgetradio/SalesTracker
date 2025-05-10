using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable CS8981
namespace SalesTracker.Migrations
{
    public partial class discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discount",
                table: "EditionsETL",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discount",
                table: "Editions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "EditionsETL");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Editions");
        }
    }
}
