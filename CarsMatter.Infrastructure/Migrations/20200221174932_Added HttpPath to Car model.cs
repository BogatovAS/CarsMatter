using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsMatter.Infrastructure.Migrations
{
    public partial class AddedHttpPathtoCarmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HttpPath",
                table: "Cars",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HttpPath",
                table: "Cars");
        }
    }
}
