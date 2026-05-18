using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Purchases.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPurchasesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_purchases_company_id",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_purchases_supplier_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "supplier",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "purchases",
                table: "purchase_items");

            migrationBuilder.RenameColumn(
                name: "date",
                schema: "purchases",
                table: "purchases",
                newName: "created_at");

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                schema: "purchases",
                table: "purchases",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "warehouse_cen",
                schema: "purchases",
                table: "purchases",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "product_cen",
                schema: "purchases",
                table: "purchase_items",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "purchases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    supplier_cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_company_id_supplier_cen",
                schema: "purchases",
                table: "suppliers",
                columns: new[] { "company_id", "supplier_cen" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "purchases");

            migrationBuilder.DropColumn(
                name: "confirmed_at",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "warehouse_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "product_cen",
                schema: "purchases",
                table: "purchase_items");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "purchases",
                table: "purchases",
                newName: "date");

            migrationBuilder.AddColumn<string>(
                name: "supplier",
                schema: "purchases",
                table: "purchases",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "product_id",
                schema: "purchases",
                table: "purchase_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_purchases_company_id",
                schema: "purchases",
                table: "purchases",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_purchases_supplier_cen",
                schema: "purchases",
                table: "purchases",
                column: "supplier_cen");
        }
    }
}
