using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitCen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "unit_cen",
                schema: "inventory",
                table: "units",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // 2. Actualizar datos existentes (Evita que el índice explote por valores vacíos)
            migrationBuilder.Sql("UPDATE inventory.units SET unit_cen = 'UNI-' || id::text WHERE unit_cen = '';");

            // 3. Crear el índice único
            migrationBuilder.CreateIndex(
                name: "ix_units_company_id_unit_cen",
                schema: "inventory",
                table: "units",
                columns: new[] { "company_id", "unit_cen" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_units_company_id_unit_cen",
                schema: "inventory",
                table: "units");

            migrationBuilder.DropColumn(
                name: "unit_cen",
                schema: "inventory",
                table: "units");
        }
    }
}
