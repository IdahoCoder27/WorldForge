using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldForge.Web.Migrations
{
    /// <inheritdoc />
    public partial class MakeBookIdNullableInWorldNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorldNotes_Books_BookId",
                table: "WorldNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "WorldNotes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "WorldNotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorldNotes_Books_BookId",
                table: "WorldNotes",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorldNotes_Books_BookId",
                table: "WorldNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "WorldNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "WorldNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorldNotes_Books_BookId",
                table: "WorldNotes",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
