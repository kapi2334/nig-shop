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
                    kraj = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    nip = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    imie = table.Column<string>(type: "text", nullable: false),
                    nazwisko = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clients_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    nazwa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "firmaadres",
                columns: table => new
                {
                    firma_id = table.Column<int>(type: "integer", nullable: false),
                    adres_id = table.Column<int>(type: "integer", nullable: false),
                    AdresId = table.Column<int>(type: "integer", nullable: false),
                    FirmaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firmaadres", x => new { x.firma_id, x.adres_id });
                    table.ForeignKey(
                        name: "FK_firmaadres_Users_firma_id",
                        column: x => x.firma_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_firmaadres_adres_adres_id",
                        column: x => x.adres_id,
                        principalTable: "adres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "osobafizycznaadres",
                columns: table => new
                {
                    osoba_id = table.Column<int>(type: "integer", nullable: false),
                    adres_id = table.Column<int>(type: "integer", nullable: false),
                    OsobaId = table.Column<int>(type: "integer", nullable: false),
                    AdresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_osobafizycznaadres", x => new { x.osoba_id, x.adres_id });
                    table.ForeignKey(
                        name: "FK_osobafizycznaadres_Users_OsobaId",
                        column: x => x.OsobaId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_osobafizycznaadres_adres_adres_id",
                        column: x => x.adres_id,
                        principalTable: "adres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_firmaadres_adres_id",
                table: "firmaadres",
                column: "adres_id");

            migrationBuilder.CreateIndex(
                name: "IX_osobafizycznaadres_adres_id",
                table: "osobafizycznaadres",
                column: "adres_id");

            migrationBuilder.CreateIndex(
                name: "IX_osobafizycznaadres_OsobaId",
                table: "osobafizycznaadres",
                column: "OsobaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "firmaadres");

            migrationBuilder.DropTable(
                name: "osobafizycznaadres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "adres");
        }
    }
}
