using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSalesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.CreateTable(
                name: "tickets",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    ticket_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    waiter_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    customer_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    customer_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_rate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    payment_method = table.Column<int>(type: "integer", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comanda_items",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_id = table.Column<int>(type: "integer", nullable: false),
                    ticket_item_id = table.Column<int>(type: "integer", nullable: false),
                    station = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comanda_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_comanda_items_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "sales",
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_items",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    note = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_items_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "sales",
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_comanda_items_ticket_id",
                schema: "sales",
                table: "comanda_items",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_items_ticket_id",
                schema: "sales",
                table: "ticket_items",
                column: "ticket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comanda_items",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "ticket_items",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "tickets",
                schema: "sales");
        }
    }
}
