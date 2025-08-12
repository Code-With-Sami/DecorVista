using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decor_Vista.Migrations
{
    /// <inheritdoc />
    public partial class updatedesigner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Designer",
                table: "Designer");

            migrationBuilder.RenameTable(
                name: "Designer",
                newName: "Designers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Designers",
                table: "Designers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Designers",
                table: "Designers");

            migrationBuilder.RenameTable(
                name: "Designers",
                newName: "Designer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Designer",
                table: "Designer",
                column: "Id");
        }
    }
}
