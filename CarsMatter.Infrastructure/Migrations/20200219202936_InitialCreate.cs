using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsMatter.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HttpPath = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumablesNotes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    KindOfService = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Odo = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumablesNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefillNotes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Petrol = table.Column<float>(nullable: false),
                    Odo = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefillNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandModels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HttpPath = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    BrandId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandModels_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CarName = table.Column<string>(nullable: true),
                    CarImagePath = table.Column<string>(nullable: true),
                    Base64CarImage = table.Column<string>(nullable: true),
                    LowPrice = table.Column<float>(nullable: false),
                    HighPrice = table.Column<float>(nullable: false),
                    ManufactureStartDate = table.Column<string>(nullable: true),
                    ManufactureEndDate = table.Column<string>(nullable: true),
                    AvitoUri = table.Column<string>(nullable: true),
                    BodyType = table.Column<string>(nullable: true),
                    BrandModelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_BrandModels_BrandModelId",
                        column: x => x.BrandModelId,
                        principalTable: "BrandModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteCars",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CarId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteCars_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteCars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandModels_BrandId",
                table: "BrandModels",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandModelId",
                table: "Cars",
                column: "BrandModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCars_CarId",
                table: "FavoriteCars",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCars_UserId",
                table: "FavoriteCars",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumablesNotes");

            migrationBuilder.DropTable(
                name: "FavoriteCars");

            migrationBuilder.DropTable(
                name: "RefillNotes");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BrandModels");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
