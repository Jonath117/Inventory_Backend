using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMinStockToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "abbreviation",
                schema: "inventory",
                table: "units",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "inventory",
                table: "units",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "min_stock_alert",
                schema: "inventory",
                table: "products",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "station_code",
                schema: "inventory",
                table: "products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "abbreviation",
                schema: "inventory",
                table: "units");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "inventory",
                table: "units");

            migrationBuilder.DropColumn(
                name: "station_code",
                schema: "inventory",
                table: "products");

            migrationBuilder.AlterColumn<int>(
                name: "min_stock_alert",
                schema: "inventory",
                table: "products",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
