using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    public partial class migIliskilerDegisti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deposits_BookId",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_StudentId",
                table: "Deposits");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_BookId",
                table: "Deposits",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_StudentId",
                table: "Deposits",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deposits_BookId",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_StudentId",
                table: "Deposits");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_BookId",
                table: "Deposits",
                column: "BookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_StudentId",
                table: "Deposits",
                column: "StudentId",
                unique: true);
        }
    }
}
