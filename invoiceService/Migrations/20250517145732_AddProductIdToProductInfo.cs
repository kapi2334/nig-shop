using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invoiceService.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdToProductInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "produkt_id",
                table: "produktinfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "produkt_id",
                table: "produktinfo");
        }
    }
}
