using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    public partial class migduzeltmeIlisCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Books_BookId",
                table: "Movements");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Books_BookId",
                table: "Movements",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Books_BookId",
                table: "Movements");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Books_BookId",
                table: "Movements",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
