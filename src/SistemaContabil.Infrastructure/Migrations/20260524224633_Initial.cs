using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContabil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome_cliente = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    cpf = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    senha = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ativo = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "marketplace",
                columns: table => new
                {
                    id_market = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome_market = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    valor_comissao = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace", x => x.id_market);
                });

            migrationBuilder.CreateTable(
                name: "pagamento",
                columns: table => new
                {
                    id_pag = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    metodo_pag = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    data_pag = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    taxa_pag = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagamento", x => x.id_pag);
                });

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id_prod = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome_prod = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    descr = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: false),
                    preco = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    estoque = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produto", x => x.id_prod);
                });

            migrationBuilder.CreateTable(
                name: "conta_contabil",
                columns: table => new
                {
                    id_conta = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nome_conta = table.Column<string>(type: "NVARCHAR2(70)", maxLength: 70, nullable: false),
                    tipo_conta = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    cliente_id_cliente = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conta_contabil", x => x.id_conta);
                    table.ForeignKey(
                        name: "FK_conta_contabil_cliente_cliente_id_cliente",
                        column: x => x.cliente_id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente");
                });

            migrationBuilder.CreateTable(
                name: "reg_cont",
                columns: table => new
                {
                    id_reg_cont = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    valor = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    data_lanca = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    conta_contabil_id_conta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CentroCustoId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reg_cont", x => x.id_reg_cont);
                    table.ForeignKey(
                        name: "FK_reg_cont_conta_contabil_conta_contabil_id_conta",
                        column: x => x.conta_contabil_id_conta,
                        principalTable: "conta_contabil",
                        principalColumn: "id_conta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendas",
                columns: table => new
                {
                    id_vendas = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    data_venda = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    status_venda = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    valor_bruto = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    valor_desconto = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    valor_frete = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    valor_liquido = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    valor_total = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    cliente_id_cliente = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    reg_cont_id_reg_cont = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    pagamento_id_pag = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    marketplace_id_market = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RegistroContabilId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendas", x => x.id_vendas);
                    table.ForeignKey(
                        name: "FK_vendas_cliente_cliente_id_cliente",
                        column: x => x.cliente_id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vendas_marketplace_marketplace_id_market",
                        column: x => x.marketplace_id_market,
                        principalTable: "marketplace",
                        principalColumn: "id_market",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vendas_pagamento_pagamento_id_pag",
                        column: x => x.pagamento_id_pag,
                        principalTable: "pagamento",
                        principalColumn: "id_pag");
                    table.ForeignKey(
                        name: "FK_vendas_reg_cont_RegistroContabilId",
                        column: x => x.RegistroContabilId,
                        principalTable: "reg_cont",
                        principalColumn: "id_reg_cont",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vendas_reg_cont_reg_cont_id_reg_cont",
                        column: x => x.reg_cont_id_reg_cont,
                        principalTable: "reg_cont",
                        principalColumn: "id_reg_cont",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_venda",
                columns: table => new
                {
                    id_item = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    quant = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    valor_unit = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    produto_id_prod = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    vendas_id_vendas = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_venda", x => x.id_item);
                    table.ForeignKey(
                        name: "FK_item_venda_produto_produto_id_prod",
                        column: x => x.produto_id_prod,
                        principalTable: "produto",
                        principalColumn: "id_prod",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_venda_vendas_vendas_id_vendas",
                        column: x => x.vendas_id_vendas,
                        principalTable: "vendas",
                        principalColumn: "id_vendas",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conta_contabil_cliente_id_cliente",
                table: "conta_contabil",
                column: "cliente_id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_produto_id_prod",
                table: "item_venda",
                column: "produto_id_prod");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_vendas_id_vendas",
                table: "item_venda",
                column: "vendas_id_vendas");

            migrationBuilder.CreateIndex(
                name: "IX_reg_cont_conta_contabil_id_conta",
                table: "reg_cont",
                column: "conta_contabil_id_conta");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_cliente_id_cliente",
                table: "vendas",
                column: "cliente_id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_marketplace_id_market",
                table: "vendas",
                column: "marketplace_id_market");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_pagamento_id_pag",
                table: "vendas",
                column: "pagamento_id_pag");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_reg_cont_id_reg_cont",
                table: "vendas",
                column: "reg_cont_id_reg_cont");

            migrationBuilder.CreateIndex(
                name: "IX_vendas_RegistroContabilId",
                table: "vendas",
                column: "RegistroContabilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_venda");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "vendas");

            migrationBuilder.DropTable(
                name: "marketplace");

            migrationBuilder.DropTable(
                name: "pagamento");

            migrationBuilder.DropTable(
                name: "reg_cont");

            migrationBuilder.DropTable(
                name: "conta_contabil");

            migrationBuilder.DropTable(
                name: "cliente");
        }
    }
}
