using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceRazor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeCambiaCampoTransacctionIDPosSessionIdEnOrdenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransaccionId",
                table: "Ordenes",
                newName: "SessionId");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Ordenes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Ordenes");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Ordenes",
                newName: "TransaccionId");
        }
    }
}
