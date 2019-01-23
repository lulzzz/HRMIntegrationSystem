using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Altinn.Api.Repositories.Migrations
{
    public partial class CreateAltinnNavMessagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Altinn");

            migrationBuilder.CreateTable(
                name: "NavMessages",
                schema: "Altinn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalId = table.Column<string>(maxLength: 255, nullable: false),
                    MessageXml = table.Column<string>(nullable: false),
                    Namespace = table.Column<string>(maxLength: 255, nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    WorkState = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<string>(maxLength: 255, nullable: true),
                    BusinessOrganizationNumber = table.Column<string>(maxLength: 255, nullable: false),
                    ReporteeId = table.Column<string>(maxLength: 255, nullable: false),
                    MesssageId = table.Column<string>(maxLength: 255, nullable: false),
                    AttachmentId = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavMessages",
                schema: "Altinn");
        }
    }
}
