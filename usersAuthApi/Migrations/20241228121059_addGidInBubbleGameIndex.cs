using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class addGidInBubbleGameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Tab_FundTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    GId = table.Column<int>(type: "int", nullable: true),
                    CreditAmount = table.Column<decimal>(type: "money", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "money", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxNoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab_FundTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_FundTransaction_Tab_Games_GId",
                        column: x => x.GId,
                        principalTable: "Tab_Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tab_FundTransaction_Tab_Register_PId",
                        column: x => x.PId,
                        principalTable: "Tab_Register",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
             
            migrationBuilder.CreateIndex(
                name: "IX_Tab_FundTransaction_GId",
                table: "Tab_FundTransaction",
                column: "GId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_FundTransaction_PId",
                table: "Tab_FundTransaction",
                column: "PId");
             
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "Tab_FundTransaction");
             
        }
    }
}
