using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace productService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "typnawierzchni",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typnawierzchni", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wymiary",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dlugosc = table.Column<double>(type: "double precision", nullable: false),
                    szerokosc = table.Column<double>(type: "double precision", nullable: false),
                    wysokosc = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wymiary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "produkt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "text", nullable: false),
                    rodzaj = table.Column<string>(type: "text", nullable: false),
                    waga = table.Column<double>(type: "double precision", nullable: false),
                    cena = table.Column<float>(type: "real", nullable: false),
                    podatek = table.Column<int>(type: "integer", nullable: false),
                    wymiary_id = table.Column<int>(type: "integer", nullable: false),
                    material_id = table.Column<int>(type: "integer", nullable: false),
                    typ_nawierzchni_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produkt", x => x.id);
                    table.ForeignKey(
                        name: "FK_produkt_material_material_id",
                        column: x => x.material_id,
                        principalTable: "material",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_produkt_typnawierzchni_typ_nawierzchni_id",
                        column: x => x.typ_nawierzchni_id,
                        principalTable: "typnawierzchni",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_produkt_wymiary_wymiary_id",
                        column: x => x.wymiary_id,
                        principalTable: "wymiary",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produkt_material_id",
                table: "produkt",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_produkt_typ_nawierzchni_id",
                table: "produkt",
                column: "typ_nawierzchni_id");

            migrationBuilder.CreateIndex(
                name: "IX_produkt_wymiary_id",
                table: "produkt",
                column: "wymiary_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produkt");

            migrationBuilder.DropTable(
                name: "material");

            migrationBuilder.DropTable(
                name: "typnawierzchni");

            migrationBuilder.DropTable(
                name: "wymiary");
        }
    }
}
