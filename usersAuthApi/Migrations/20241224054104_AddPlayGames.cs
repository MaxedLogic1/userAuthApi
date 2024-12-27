using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tab_PlayGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GId = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    BetAmount = table.Column<decimal>(type: "money", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false),
                    AchiveTarget = table.Column<int>(type: "int", nullable: false),
                    WinLoss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab_PlayGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_PlayGame_Tab_Games_GId",
                        column: x => x.GId,
                        principalTable: "Tab_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tab_PlayGame_Tab_Register_PId",
                        column: x => x.PId,
                        principalTable: "Tab_Register",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tab_PlayGame_GId",
                table: "Tab_PlayGame",
                column: "GId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_PlayGame_PId",
                table: "Tab_PlayGame",
                column: "PId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tab_PlayGame");
        }
    }
}
