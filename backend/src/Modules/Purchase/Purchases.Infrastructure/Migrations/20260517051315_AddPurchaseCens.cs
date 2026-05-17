using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseCens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "supplier",
                schema: "purchases",
                table: "purchases",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "order_cen",
                schema: "purchases",
                table: "purchases",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "supplier_cen",
                schema: "purchases",
                table: "purchases",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_purchases_company_id_order_cen",
                schema: "purchases",
                table: "purchases",
                columns: new[] { "company_id", "order_cen" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchases_supplier_cen",
                schema: "purchases",
                table: "purchases",
                column: "supplier_cen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_purchases_company_id_order_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "ix_purchases_supplier_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "order_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "supplier_cen",
                schema: "purchases",
                table: "purchases");

            migrationBuilder.AlterColumn<string>(
                name: "supplier",
                schema: "purchases",
                table: "purchases",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
