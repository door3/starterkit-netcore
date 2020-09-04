using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleBookstore.Services.BookService.Infrastructure.Migrations.BookDbStoreMigrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "book");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "book",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedByUser = table.Column<int>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedByUser = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ReferenceCode = table.Column<string>(nullable: true),
                    Name_Prefix = table.Column<string>(nullable: true),
                    Name_FirstName = table.Column<string>(nullable: true),
                    Name_MiddleName = table.Column<string>(nullable: true),
                    Name_LastName = table.Column<string>(nullable: true),
                    Name_Suffix = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "book",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedByUser = table.Column<int>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedByUser = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ReferenceCode = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                schema: "book",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "book",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "book",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                schema: "book",
                table: "BookAuthors",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors",
                schema: "book");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "book");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "book");
        }
    }
}
