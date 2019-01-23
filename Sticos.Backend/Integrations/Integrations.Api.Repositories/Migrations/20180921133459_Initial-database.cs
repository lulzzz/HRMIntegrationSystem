using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Integrations.Api.Repositories.Migrations
{
    public partial class Initialdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Integrations");

            migrationBuilder.CreateTable(
                name: "EntityMaps",
                schema: "Integrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IntegrationId = table.Column<int>(nullable: false),
                    EntityName = table.Column<string>(nullable: true),
                    EntityId = table.Column<int>(nullable: false),
                    ExternalValue = table.Column<string>(nullable: true),
                    ExternalEntity = table.Column<string>(nullable: true),
                    ExternalPropertyName = table.Column<string>(nullable: true),
                    Ignored = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Integrations",
                schema: "Integrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsValid = table.Column<bool>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    ExternalSystem = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityMaps",
                schema: "Integrations");

            migrationBuilder.DropTable(
                name: "Integrations",
                schema: "Integrations");
        }
    }
}
