using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlzaProductApi.Data.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "_timestamp",
                table: "Products",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_timestamp",
                table: "Products");
        }
    }
}
