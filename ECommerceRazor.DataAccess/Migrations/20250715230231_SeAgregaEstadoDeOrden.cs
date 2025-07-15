using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceRazor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeAgregaEstadoDeOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Ordenes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Ordenes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Ordenes");
        }
    }
}
