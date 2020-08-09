using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlzaProductApi.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ImgUri = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "SSD MVE", "http:\\temp.uri", "Hardrive", 2000.89m },
                    { 2, "SSD MVE", "http:\\temp.uri", "Hardrive", 2000.89m },
                    { 3, "SSD MVE", "http:\\temp.uri", "Hardrive", 2000.89m },
                    { 4, "SSD MVE", "http:\\temp.uri", "Hardrive", 2000.89m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
