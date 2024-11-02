using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    public partial class migoptionTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageSize",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirOgrenciKacKitapOkuyabilir = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropColumn(
                name: "PageSize",
                table: "Books");
        }
    }
}
