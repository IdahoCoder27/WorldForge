using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldForge.Web.Migrations
{
    /// <inheritdoc />
    public partial class MakeCharacterBookIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Books_BookId",
                table: "Characters");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Characters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Books_BookId",
                table: "Characters",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Books_BookId",
                table: "Characters");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Books_BookId",
                table: "Characters",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
