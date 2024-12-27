using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class removesomecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchiveTarget",
                table: "Tab_PlayGame");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "Tab_PlayGame");

            migrationBuilder.AddColumn<string>(
                name: "RandomId",
                table: "Tab_PlayGame",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomId",
                table: "Tab_PlayGame");

            migrationBuilder.AddColumn<int>(
                name: "AchiveTarget",
                table: "Tab_PlayGame",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Target",
                table: "Tab_PlayGame",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
