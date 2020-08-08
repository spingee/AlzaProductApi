using Microsoft.EntityFrameworkCore.Migrations;

namespace AlzaProductApi.Data.Migrations
{
    public partial class InitialCreate : Migration
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
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false)
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
                    { 1, "SportlineDescription", "http:\\temp.uri", "Sportline", 299000.9m },
                    { 2, "RsDescription", "http:\\temp.uri", "RS", 599999m },
                    { 3, "ActiveDescription", "http:\\temp.uri", "Active", 180000m },
                    { 4, "AmbitionDescription", "http:\\temp.uri", "Ambition", 423123m },
                    { 5, "ActiveFabiaDescription", "http:\\temp.uri", "ActiveFabia", 123000m },
                    { 6, "AmbitionFabiaDescription", "http:\\temp.uri", "AmbitionFabia", 333333m },
                    { 7, "AmbitionOctaviaDescription", "http:\\temp.uri", "AmbitionOctavia", 350000m },
                    { 8, "StyleOctaviaDescription", "http:\\temp.uri", "StyleOctavia", 699000m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
