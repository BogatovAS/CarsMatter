using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsMatter.Infrastructure.Migrations
{
    public partial class IgnoredBase64CarImageCarfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64CarImage",
                table: "Cars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Base64CarImage",
                table: "Cars",
                nullable: true);
        }
    }
}
