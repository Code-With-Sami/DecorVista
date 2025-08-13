using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decor_Vista.Migrations
{
    /// <inheritdoc />
    public partial class AddImgToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Users");
        }
    }
}
