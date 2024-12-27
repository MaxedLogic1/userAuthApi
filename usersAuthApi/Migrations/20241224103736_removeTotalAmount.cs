using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace usersAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class removeTotalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Tab_Register");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Tab_Register",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
