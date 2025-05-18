using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orderService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zamowienie",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_klienta = table.Column<int>(type: "integer", nullable: false),
                    dostawa = table.Column<bool>(type: "boolean", nullable: false),
                    id_zamowienia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zamowienie", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zamowienie_produkty",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    zamowienie_id = table.Column<int>(type: "integer", nullable: false),
                    produkt_id = table.Column<int>(type: "integer", nullable: false),
                    ilosc = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zamowienie_produkty", x => x.id);
                    table.ForeignKey(
                        name: "FK_zamowienie_produkty_zamowienie_zamowienie_id",
                        column: x => x.zamowienie_id,
                        principalTable: "zamowienie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_zamowienie_produkty_zamowienie_id",
                table: "zamowienie_produkty",
                column: "zamowienie_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zamowienie_produkty");

            migrationBuilder.DropTable(
                name: "zamowienie");
        }
    }
}
