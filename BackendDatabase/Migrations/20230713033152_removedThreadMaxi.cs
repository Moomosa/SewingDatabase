using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendDatabase.Migrations
{
    public partial class removedThreadMaxi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxiLockStretch",
                table: "Thread");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MaxiLockStretch",
                table: "Thread",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
