﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldForge.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryToWorldNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "WorldNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "WorldNotes");
        }
    }
}
