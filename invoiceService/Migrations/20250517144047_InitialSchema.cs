using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invoiceService.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faktura_wystawca_IssuerId",
                table: "faktura");

            migrationBuilder.DropForeignKey(
                name: "FK_produktinfo_faktura_InvoiceId",
                table: "produktinfo");

            migrationBuilder.DropIndex(
                name: "IX_produktinfo_InvoiceId",
                table: "produktinfo");

            migrationBuilder.DropIndex(
                name: "IX_faktura_IssuerId",
                table: "faktura");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "produktinfo");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "faktura");

            migrationBuilder.CreateIndex(
                name: "IX_produktinfo_faktura_id",
                table: "produktinfo",
                column: "faktura_id");

            migrationBuilder.CreateIndex(
                name: "IX_faktura_wystawca_id",
                table: "faktura",
                column: "wystawca_id");

            migrationBuilder.AddForeignKey(
                name: "FK_faktura_wystawca_wystawca_id",
                table: "faktura",
                column: "wystawca_id",
                principalTable: "wystawca",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produktinfo_faktura_faktura_id",
                table: "produktinfo",
                column: "faktura_id",
                principalTable: "faktura",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faktura_wystawca_wystawca_id",
                table: "faktura");

            migrationBuilder.DropForeignKey(
                name: "FK_produktinfo_faktura_faktura_id",
                table: "produktinfo");

            migrationBuilder.DropIndex(
                name: "IX_produktinfo_faktura_id",
                table: "produktinfo");

            migrationBuilder.DropIndex(
                name: "IX_faktura_wystawca_id",
                table: "faktura");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "produktinfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "faktura",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_produktinfo_InvoiceId",
                table: "produktinfo",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_faktura_IssuerId",
                table: "faktura",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_faktura_wystawca_IssuerId",
                table: "faktura",
                column: "IssuerId",
                principalTable: "wystawca",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produktinfo_faktura_InvoiceId",
                table: "produktinfo",
                column: "InvoiceId",
                principalTable: "faktura",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
