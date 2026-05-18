using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseAndMovementCens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_warehouses_company_id",
                schema: "inventory",
                table: "warehouses");

            migrationBuilder.DropIndex(
                name: "ix_inventory_movements_company_id",
                schema: "inventory",
                table: "inventory_movements");

            migrationBuilder.AddColumn<string>(
                name: "warehouse_cen",
                schema: "inventory",
                table: "warehouses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_stock",
                schema: "inventory",
                table: "inventory_stock",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "previous_stock",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "new_stock",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "movement_cen",
                schema: "inventory",
                table: "inventory_movements",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
            
            migrationBuilder.Sql("UPDATE inventory.warehouses SET warehouse_cen = 'WH-' || id::text WHERE warehouse_cen = '';");
            migrationBuilder.Sql("UPDATE inventory.inventory_movements SET movement_cen = 'MOV-' || id::text WHERE movement_cen = '';");

            migrationBuilder.CreateIndex(
                name: "ix_warehouses_company_id_warehouse_cen",
                schema: "inventory",
                table: "warehouses",
                columns: new[] { "company_id", "warehouse_cen" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_inventory_movements_company_id_movement_cen",
                schema: "inventory",
                table: "inventory_movements",
                columns: new[] { "company_id", "movement_cen" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_warehouses_company_id_warehouse_cen",
                schema: "inventory",
                table: "warehouses");

            migrationBuilder.DropIndex(
                name: "ix_inventory_movements_company_id_movement_cen",
                schema: "inventory",
                table: "inventory_movements");

            migrationBuilder.DropColumn(
                name: "warehouse_cen",
                schema: "inventory",
                table: "warehouses");

            migrationBuilder.DropColumn(
                name: "movement_cen",
                schema: "inventory",
                table: "inventory_movements");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_stock",
                schema: "inventory",
                table: "inventory_stock",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "previous_stock",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "new_stock",
                schema: "inventory",
                table: "inventory_movements",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.CreateIndex(
                name: "ix_warehouses_company_id",
                schema: "inventory",
                table: "warehouses",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_movements_company_id",
                schema: "inventory",
                table: "inventory_movements",
                column: "company_id");
        }
    }
}
