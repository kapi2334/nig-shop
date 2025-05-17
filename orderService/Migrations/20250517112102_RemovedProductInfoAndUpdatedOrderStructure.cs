using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace orderService.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProductInfoAndUpdatedOrderStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ilosc",
                table: "zamowienie_produkty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ilosc",
                table: "zamowienie_produkty",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
