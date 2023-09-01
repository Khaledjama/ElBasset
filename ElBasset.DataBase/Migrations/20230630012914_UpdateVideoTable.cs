using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElBasset.DataBase.Migrations
{
    public partial class UpdateVideoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageVideoPath",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageVideoPath",
                table: "Videos");
        }
    }
}
