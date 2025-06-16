using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreManagement_1.Migrations
{
    /// <inheritdoc />
    public partial class mig_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "UserLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "UserLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "UserLogs");
        }
    }
}
