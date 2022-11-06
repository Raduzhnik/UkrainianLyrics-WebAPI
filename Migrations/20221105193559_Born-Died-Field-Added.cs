using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UkrainianLyrics.WebAPI.Migrations
{
    public partial class BornDiedFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lived",
                table: "Authors",
                newName: "Died");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Born",
                table: "Authors",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Born",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "Died",
                table: "Authors",
                newName: "Lived");
        }
    }
}
