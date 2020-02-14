using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    approved = table.Column<bool>(type: "bit", nullable: false),
                    import_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    amount_rows = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "varchar(128)", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(256)", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreviousImportItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", nullable: false),
                    email = table.Column<string>(type: "varchar(256)", nullable: false),
                    birth_date = table.Column<string>(type: "varchar(16)", nullable: false),
                    gender = table.Column<string>(type: "varchar(16)", nullable: false),
                    status = table.Column<int>(nullable: false),
                    import_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousImportItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_PreviousImportItem_Import_import_id",
                        column: x => x.import_id,
                        principalTable: "Import",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousImportItem_import_id",
                table: "PreviousImportItem",
                column: "import_id");

            migrationBuilder.CreateIndex(
                name: "uq_email",
                table: "User",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousImportItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Import");
        }
    }
}
