using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetimeInFlappyBirdGameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TotalScoreTime",
                table: "Tab_FlappyBirdGameIndex",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TotalScoreTime",
                table: "Tab_FlappyBirdGameIndex",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }
    }
}
