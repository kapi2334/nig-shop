using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace userService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "podmiot",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false),
                    nip = table.Column<long>(type: "bigint", nullable: true),
                    type = table.Column<char>(type: "character(1)", nullable: false),
                    imie = table.Column<string>(type: "text", nullable: false),
                    nazwisko = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_podmiot", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "adres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ulica = table.Column<string>(type: "text", nullable: false),
                    nrdomu = table.Column<int>(type: "integer", nullable: false),
                    nrmieszkania = table.Column<int>(type: "integer", nullable: true),
                    kodpocztowy = table.Column<string>(type: "text", nullable: false),
                    miasto = table.Column<string>(type: "text", nullable: false),
                    kraj = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adres", x => x.id);
                    table.ForeignKey(
                        name: "FK_adres_podmiot_UserId",
                        column: x => x.UserId,
                        principalTable: "podmiot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adres_UserId",
                table: "adres",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adres");

            migrationBuilder.DropTable(
                name: "podmiot");
        }
    }
}
