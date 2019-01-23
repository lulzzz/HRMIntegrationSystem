using Microsoft.EntityFrameworkCore.Migrations;

namespace Integrations.Api.Repositories.Migrations
{
    public partial class isActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsValid",
                schema: "Integrations",
                table: "Integrations",
                newName: "IsActivated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActivated",
                schema: "Integrations",
                table: "Integrations",
                newName: "IsValid");
        }
    }
}
