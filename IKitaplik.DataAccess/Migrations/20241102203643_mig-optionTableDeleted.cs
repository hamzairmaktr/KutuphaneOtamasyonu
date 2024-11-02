using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    public partial class migoptionTableDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
