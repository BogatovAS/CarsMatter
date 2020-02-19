using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsMatter.Infrastructure.Migrations
{
    public partial class Changedthecarpricestype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LowPrice",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "HighPrice",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "LowPrice",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "HighPrice",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
