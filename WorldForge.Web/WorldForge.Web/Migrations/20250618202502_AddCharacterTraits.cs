using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldForge.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterTraits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomDescription",
                table: "CharacterTraits",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomDescription",
                table: "CharacterTraits");
        }
    }
}
