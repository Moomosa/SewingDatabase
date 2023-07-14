using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendDatabase.Migrations
{
    public partial class changedMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PurchasePrice",
                table: "Machine",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastServiced",
                table: "Machine",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastServiced",
                table: "Machine");

            migrationBuilder.AlterColumn<float>(
                name: "PurchasePrice",
                table: "Machine",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
