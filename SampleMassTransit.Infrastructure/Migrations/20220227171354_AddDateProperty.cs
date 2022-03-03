using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleMassTransit.Infrastructure.Migrations
{
    public partial class AddDateProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNumber",
                table: "OrderState",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitDate",
                table: "OrderState",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "OrderState",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "OrderState");

            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "OrderState");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "OrderState");
        }
    }
}
