using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFlapplyBirdGameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tab_FlappyBirdGameIndex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    GId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: true),
                    BetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAchiveScore = table.Column<int>(type: "int", nullable: false),
                    TotalScoreTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Win = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Loss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RandomId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab_FlappyBirdGameIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_FlappyBirdGameIndex_Tab_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Tab_Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tab_FlappyBirdGameIndex_Tab_Register_UserId",
                        column: x => x.UserId,
                        principalTable: "Tab_Register",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tab_FlappyBirdGameIndex_GamesId",
                table: "Tab_FlappyBirdGameIndex",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_FlappyBirdGameIndex_UserId",
                table: "Tab_FlappyBirdGameIndex",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tab_FlappyBirdGameIndex");
        }
    }
}
