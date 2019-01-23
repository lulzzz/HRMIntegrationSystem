using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Timereg.Api.Repositories.Migrations
{
    public partial class Addedcreatedandupdatecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "Timereg",
                table: "AbsenceExports",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Timereg",
                table: "AbsenceExports",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateAt",
                schema: "Timereg",
                table: "AbsenceExports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "Timereg",
                table: "AbsenceExports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Timereg",
                table: "AbsenceExports");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Timereg",
                table: "AbsenceExports");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                schema: "Timereg",
                table: "AbsenceExports");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Timereg",
                table: "AbsenceExports");
        }
    }
}
