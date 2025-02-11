using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealSpot.Data.migrations
{
    /// <inheritdoc />
    public partial class MakeNLastrejectedPriceTimeNullableAndAddNegotiationPriceProposedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastRejectedPriceTime",
                table: "Negotiation",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "NegotiationPriceProposedTime",
                table: "Negotiation",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegotiationPriceProposedTime",
                table: "Negotiation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastRejectedPriceTime",
                table: "Negotiation",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
