using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsMatter.Infrastructure.Migrations
{
    public partial class RefactoredFavoriteCarsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCars_Cars_CarId",
                table: "FavoriteCars");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCars_Users_UserId",
                table: "FavoriteCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteCars",
                table: "FavoriteCars");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteCars_CarId",
                table: "FavoriteCars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FavoriteCars");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavoriteCars",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CarId",
                table: "FavoriteCars",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteCars",
                table: "FavoriteCars",
                columns: new[] { "CarId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCars_Cars_CarId",
                table: "FavoriteCars",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCars_Users_UserId",
                table: "FavoriteCars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCars_Cars_CarId",
                table: "FavoriteCars");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCars_Users_UserId",
                table: "FavoriteCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteCars",
                table: "FavoriteCars");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavoriteCars",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CarId",
                table: "FavoriteCars",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "FavoriteCars",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteCars",
                table: "FavoriteCars",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCars_CarId",
                table: "FavoriteCars",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCars_Cars_CarId",
                table: "FavoriteCars",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCars_Users_UserId",
                table: "FavoriteCars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
