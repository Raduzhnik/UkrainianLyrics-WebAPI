using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UkrainianLyrics.WebAPI.Migrations
{
    public partial class DelCompsitionCreationDateAddedDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Born",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Died",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Compositons",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Compositons",
                newName: "CreationDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Born",
                table: "Authors",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Died",
                table: "Authors",
                type: "TEXT",
                nullable: true);
        }
    }
}
