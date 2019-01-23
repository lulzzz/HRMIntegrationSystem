using Microsoft.EntityFrameworkCore.Migrations;

namespace Timereg.Api.Repositories.Migrations
{
    public partial class ActionOnAbsenceExport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Action",
                schema: "Timereg",
                table: "AbsenceExports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                schema: "Timereg",
                table: "AbsenceExports");
        }
    }
}
