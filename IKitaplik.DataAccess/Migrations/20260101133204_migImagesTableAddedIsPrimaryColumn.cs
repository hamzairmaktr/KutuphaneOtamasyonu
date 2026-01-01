using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migImagesTableAddedIsPrimaryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Images");
        }
    }
}
