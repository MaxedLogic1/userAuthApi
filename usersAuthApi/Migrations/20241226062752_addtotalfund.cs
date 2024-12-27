using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class addtotalfund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tab_PlayerTotalFund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PId = table.Column<int>(type: "int", nullable: false),
                    TotalFund = table.Column<decimal>(type: "money", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RandomId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab_PlayerTotalFund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_PlayerTotalFund_Tab_Register_PId",
                        column: x => x.PId,
                        principalTable: "Tab_Register",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tab_PlayerTotalFund_PId",
                table: "Tab_PlayerTotalFund",
                column: "PId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tab_PlayerTotalFund");
        }
    }
}
