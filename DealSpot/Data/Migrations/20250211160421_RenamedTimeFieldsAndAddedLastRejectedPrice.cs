using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealSpot.Data.migrations
{
    /// <inheritdoc />
    public partial class RenamedTimeFieldsAndAddedLastRejectedPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NegotiationStartTime",
                table: "Negotiation",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "NegotiationPriceProposedTime",
                table: "Negotiation",
                newName: "PriceProposedTime");

            migrationBuilder.RenameColumn(
                name: "LastRejectedPriceTime",
                table: "Negotiation",
                newName: "LastRejectedTime");

            migrationBuilder.AddColumn<decimal>(
                name: "LastRejectedPrice",
                table: "Negotiation",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRejectedPrice",
                table: "Negotiation");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Negotiation",
                newName: "NegotiationStartTime");

            migrationBuilder.RenameColumn(
                name: "PriceProposedTime",
                table: "Negotiation",
                newName: "NegotiationPriceProposedTime");

            migrationBuilder.RenameColumn(
                name: "LastRejectedTime",
                table: "Negotiation",
                newName: "LastRejectedPriceTime");
        }
    }
}
