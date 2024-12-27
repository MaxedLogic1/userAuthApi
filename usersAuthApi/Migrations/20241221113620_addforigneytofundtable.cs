using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class addforigneytofundtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "Tab_FundTransaction",
                newName: "DebitAmount");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "Tab_FundTransaction",
                newName: "CreditAmount");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_FundTransaction_PId",
                table: "Tab_FundTransaction",
                column: "PId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tab_FundTransaction_Tab_Register_PId",
                table: "Tab_FundTransaction",
                column: "PId",
                principalTable: "Tab_Register",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tab_FundTransaction_Tab_Register_PId",
                table: "Tab_FundTransaction");

            migrationBuilder.DropIndex(
                name: "IX_Tab_FundTransaction_PId",
                table: "Tab_FundTransaction");

            migrationBuilder.RenameColumn(
                name: "DebitAmount",
                table: "Tab_FundTransaction",
                newName: "Debit");

            migrationBuilder.RenameColumn(
                name: "CreditAmount",
                table: "Tab_FundTransaction",
                newName: "Credit");
        }
    }
}
