using Microsoft.EntityFrameworkCore.Migrations;

namespace Timereg.Api.Repositories.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Timereg");

            migrationBuilder.CreateTable(
                name: "AbsenceExports",
                schema: "Timereg",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    LocalAbsenceId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    AbsenceJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceExports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsenceExports",
                schema: "Timereg");
        }
    }
}
