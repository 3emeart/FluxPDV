using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMercadoSaas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Promotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promocaos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocaos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegraPromocaos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromocaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdutoId = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeMinima = table.Column<int>(type: "integer", nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    QuantidadePaga = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegraPromocaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegraPromocaos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegraPromocaos_Promocaos_PromocaoId",
                        column: x => x.PromocaoId,
                        principalTable: "Promocaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegraPromocaos_ProdutoId",
                table: "RegraPromocaos",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegraPromocaos_PromocaoId",
                table: "RegraPromocaos",
                column: "PromocaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegraPromocaos");

            migrationBuilder.DropTable(
                name: "Promocaos");
        }
    }
}
