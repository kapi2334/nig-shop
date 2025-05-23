﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace invoiceService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produkt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "text", nullable: false),
                    typ = table.Column<string>(type: "text", nullable: false),
                    waga = table.Column<double>(type: "double precision", nullable: false),
                    cena = table.Column<float>(type: "real", nullable: false),
                    podatek = table.Column<int>(type: "integer", nullable: false),
                    wymiary_id = table.Column<int>(type: "integer", nullable: false),
                    material_id = table.Column<int>(type: "integer", nullable: false),
                    powierzchnia_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produkt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wystawca",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa = table.Column<string>(type: "text", nullable: false),
                    ulica = table.Column<string>(type: "text", nullable: false),
                    nrdomu = table.Column<int>(type: "integer", nullable: false),
                    nrmieszkania = table.Column<int>(type: "integer", nullable: true),
                    kodpocztowy = table.Column<string>(type: "text", nullable: false),
                    miasto = table.Column<string>(type: "text", nullable: false),
                    kraj = table.Column<string>(type: "text", nullable: false),
                    nip = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wystawca", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faktura",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    klient_nazwa = table.Column<string>(type: "text", nullable: false),
                    klient_ulica = table.Column<string>(type: "text", nullable: false),
                    klient_nrdomu = table.Column<int>(type: "integer", nullable: false),
                    klient_nrmieszkania = table.Column<int>(type: "integer", nullable: true),
                    klient_kodpocztowy = table.Column<string>(type: "text", nullable: false),
                    klient_miasto = table.Column<string>(type: "text", nullable: false),
                    klient_kraj = table.Column<string>(type: "text", nullable: false),
                    klient_nip = table.Column<long>(type: "bigint", nullable: true),
                    datawystawienia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    terminplatnosci = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rodzajplatnosci = table.Column<string>(type: "text", nullable: false),
                    wystawca_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faktura", x => x.id);
                    table.ForeignKey(
                        name: "FK_faktura_wystawca_wystawca_id",
                        column: x => x.wystawca_id,
                        principalTable: "wystawca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "produktinfo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ilosc = table.Column<string>(type: "text", nullable: false),
                    cenaogolem = table.Column<double>(type: "double precision", nullable: false),
                    podatek = table.Column<int>(type: "integer", nullable: false),
                    kwotapodatku = table.Column<double>(type: "double precision", nullable: false),
                    netto = table.Column<double>(type: "double precision", nullable: false),
                    brutto = table.Column<double>(type: "double precision", nullable: false),
                    faktura_id = table.Column<int>(type: "integer", nullable: false),
                    produkt_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produktinfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_produktinfo_faktura_faktura_id",
                        column: x => x.faktura_id,
                        principalTable: "faktura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_faktura_wystawca_id",
                table: "faktura",
                column: "wystawca_id");

            migrationBuilder.CreateIndex(
                name: "IX_produktinfo_faktura_id",
                table: "produktinfo",
                column: "faktura_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produkt");

            migrationBuilder.DropTable(
                name: "produktinfo");

            migrationBuilder.DropTable(
                name: "faktura");

            migrationBuilder.DropTable(
                name: "wystawca");
        }
    }
}
