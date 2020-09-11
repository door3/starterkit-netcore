using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleBookstore.Services.BookService.Infrastructure.Migrations.BookDbStoreMigrations
{
    public partial class AddIsbnToBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_Prefix",
                schema: "book",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Name_Suffix",
                schema: "book",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                schema: "book",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isbn",
                schema: "book",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Name_Prefix",
                schema: "book",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_Suffix",
                schema: "book",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
