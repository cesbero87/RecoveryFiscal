using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecoveryFiscal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "processos_recuperacao_fiscal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    numero_processo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nif_cliente = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_cliente = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_credito = table.Column<int>(type: "int", nullable: false),
                    valor_original_credito = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    valor_recuperavel_estimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    data_constituicao_credito = table.Column<DateOnly>(type: "date", nullable: false),
                    data_analise = table.Column<DateOnly>(type: "date", nullable: false),
                    status_processo = table.Column<int>(type: "int", nullable: false),
                    prioridade = table.Column<int>(type: "int", nullable: false),
                    observacoes = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    criado_em_utc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    atualizado_em_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    removido_em_utc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    removido_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processos_recuperacao_fiscal", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "IX_processos_recuperacao_fiscal_ativo",
                table: "processos_recuperacao_fiscal",
                column: "ativo");

            migrationBuilder.CreateIndex(
                name: "IX_processos_recuperacao_fiscal_nif_cliente",
                table: "processos_recuperacao_fiscal",
                column: "nif_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_processos_recuperacao_fiscal_nome_cliente",
                table: "processos_recuperacao_fiscal",
                column: "nome_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_processos_recuperacao_fiscal_numero_processo",
                table: "processos_recuperacao_fiscal",
                column: "numero_processo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_processos_recuperacao_fiscal_status_processo_prioridade_data~",
                table: "processos_recuperacao_fiscal",
                columns: new[] { "status_processo", "prioridade", "data_analise" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processos_recuperacao_fiscal");
        }
    }
}
