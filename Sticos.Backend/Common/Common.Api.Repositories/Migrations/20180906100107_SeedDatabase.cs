using Microsoft.EntityFrameworkCore.Migrations;

namespace Common.Api.Repositories.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Common].[OwnerTypes] (Name, Priority) VALUES ('Default', 3)");
            migrationBuilder.Sql("INSERT INTO [Common].[OwnerTypes] (Name, Priority) VALUES ('Company', 2)");
            migrationBuilder.Sql("INSERT INTO [Common].[OwnerTypes] (Name, Priority) VALUES ('User', 1)");

            migrationBuilder.Sql(
                "INSERT INTO [Common].[Dashboards] (DashboardConfig, OwnerTypeId, Title, IsDefault) VALUES ('[{\"widgetName\":\"UserWidget\",\"headerName\":\"User\",\"options\":{\"defaultHeight\":2,\"defaultWidth\":4,\"minHeight\":2,\"minWidth\":3},\"displayName\":\"UserWidget\",\"widgetId\":\"7361f728-39c9-7b62-4cf8-12bd7c753c39\",\"xXl\":0,\"yXl\":0,\"x\":0,\"y\":0,\"w\":3,\"h\":4,\"xSm\":0,\"ySm\":0,\"xMd\":0,\"yMd\":0,\"xLg\":0,\"yLg\":0,\"settings\":{\"id\":2,\"firstName\":\"Tarik\",\"lastName\":\"Eminagic\",\"email\":\"tarik@sticos.no\",\"location\":\"Sarajevo\",\"jobPosition\":\"Software Engineer\",\"image\":\"assets/img/employees/2.png\",\"customerId\":null},\"wMd\":6,\"wLg\":4},{\"widgetName\":\"WeatherWidget\",\"headerName\":\"Weather\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":3,\"minHeight\":4,\"minWidth\":3},\"displayName\":\"WeatherWidget\",\"widgetId\":\"024e5c88-4a82-7ba8-4642-cb777fc29ae4\",\"xXl\":3,\"yXl\":0,\"x\":0,\"y\":4,\"w\":3,\"h\":4,\"xSm\":0,\"ySm\":4,\"xMd\":6,\"yMd\":0,\"xLg\":8,\"yLg\":0,\"settings\":{\"location\":{\"lat\":43.8562586,\"lng\":18.4130763},\"formattedAddress\":\"Sarajevo, Bosnia and Herzegovina\",\"name\":\"Sarajevo, BA\",\"placeId\":\"ChIJ0Ztx7bHLWEcRPrOH3qbNLlY\",\"settingsSkip\":false},\"wMd\":6,\"wLg\":4},{\"widgetName\":\"EmployeeWidget\",\"headerName\":\"Employee\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":6,\"minHeight\":4,\"minWidth\":6},\"displayName\":\"EmployeeWidget\",\"widgetId\":\"2b78174c-1b18-4669-130b-166eb03d6680\",\"xXl\":6,\"yXl\":0,\"x\":0,\"y\":8,\"w\":6,\"h\":4,\"xSm\":0,\"ySm\":8,\"xMd\":0,\"yMd\":4,\"xLg\":0,\"yLg\":4,\"wMd\":12,\"wLg\":12},{\"widgetName\":\"AnomalyWidget\",\"headerName\":\"Anomaly\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":6,\"minHeight\":4,\"minWidth\":6},\"displayName\":\"AnomalyWidget\",\"widgetId\":\"c3f5442a-c8d0-6275-6d01-b3f01c2e48f5\",\"xXl\":0,\"yXl\":4,\"x\":0,\"y\":12,\"w\":6,\"h\":4,\"xSm\":0,\"ySm\":12,\"xMd\":0,\"yMd\":8,\"xLg\":0,\"yLg\":8,\"wMd\":12,\"wLg\":12},{\"widgetName\":\"VacationBankWidget\",\"headerName\":\"Vacation bank\",\"options\":{\"defaultHeight\":3,\"defaultWidth\":4,\"minHeight\":3,\"minWidth\":4},\"displayName\":\"VacationBankWidget\",\"widgetId\":\"3f3ea2eb-b494-f8b7-4047-1e04edd351f1\",\"xXl\":8,\"yXl\":8,\"x\":0,\"y\":16,\"w\":4,\"h\":4,\"xSm\":0,\"ySm\":16,\"xMd\":0,\"yMd\":12,\"xLg\":6,\"yLg\":16,\"wMd\":6,\"wLg\":6},{\"widgetName\":\"ComicWidget\",\"headerName\":\"Comic\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":6,\"minHeight\":4,\"minWidth\":6},\"displayName\":\"ComicWidget\",\"widgetId\":\"bbacef58-08ef-2778-e0b5-c9fc83bba09b\",\"xXl\":6,\"yXl\":4,\"x\":0,\"y\":20,\"w\":6,\"h\":4,\"xSm\":0,\"ySm\":20,\"xMd\":0,\"yMd\":16,\"xLg\":0,\"yLg\":12,\"wMd\":12,\"wLg\":12},{\"widgetName\":\"NewsWidget\",\"headerName\":\"News\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":4,\"minHeight\":4,\"minWidth\":3},\"displayName\":\"NewsWidget\",\"widgetId\":\"3d57102d-f2eb-946b-2dfd-94d1596947a2\",\"xXl\":0,\"yXl\":8,\"x\":0,\"y\":24,\"w\":5,\"h\":4,\"xSm\":0,\"ySm\":24,\"xMd\":0,\"yMd\":20,\"xLg\":0,\"yLg\":16,\"wMd\":12,\"wLg\":6},{\"widgetName\":\"PresenceWidget\",\"headerName\":\"Presence\",\"options\":{\"defaultHeight\":4,\"defaultWidth\":3,\"minHeight\":4,\"minWidth\":3},\"displayName\":\"PresenceWidget\",\"widgetId\":\"7204a973-580d-144f-accd-f7a462f532dc\",\"xXl\":5,\"yXl\":8,\"x\":0,\"y\":28,\"w\":3,\"h\":4,\"xSm\":0,\"ySm\":28,\"xMd\":6,\"yMd\":12,\"xLg\":4,\"yLg\":0,\"wMd\":6,\"wLg\":4}]', 1, 'Default', 1)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
