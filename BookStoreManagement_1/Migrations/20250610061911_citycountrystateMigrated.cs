using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreManagement_1.Migrations
{
    /// <inheritdoc />
    public partial class citycountrystateMigrated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Admins",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Admins",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Admins",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Admins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_CityId",
                table: "Admins",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_CountryId",
                table: "Admins",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_StateId",
                table: "Admins",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Cities_CityId",
                table: "Admins",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Countries_CountryId",
                table: "Admins",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_States_StateId",
                table: "Admins",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Cities_CityId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Countries_CountryId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_States_StateId",
                table: "Admins");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Admins_CityId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_CountryId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_StateId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Admins",
                newName: "Username");
        }
    }
}
