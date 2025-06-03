using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldForge.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndStatusToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Characters");
        }
    }
}
