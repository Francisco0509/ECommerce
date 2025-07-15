using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceRazor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeAgregaCampoNombreUsuarioTableOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "Ordenes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "Ordenes");
        }
    }
}
