using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyCen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cen",
                schema: "core",
                table: "companies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
            
            migrationBuilder.Sql("UPDATE core.companies SET cen = 'COMP-' || id::text WHERE cen = '';");

            migrationBuilder.CreateIndex(
                name: "ix_companies_cen",
                schema: "core",
                table: "companies",
                column: "cen",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_companies_cen",
                schema: "core",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "cen",
                schema: "core",
                table: "companies");
        }
    }
}
