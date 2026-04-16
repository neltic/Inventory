using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddGlobalizationData
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts", "InitialData");

        string sqlGlobalizationData = File.ReadAllText(Path.Combine(scriptsPath, "GlobalizationData.sql"));

        migrationBuilder.Sql(sqlGlobalizationData);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE [dbo].[Translation]");
        migrationBuilder.Sql("DELETE [dbo].[Label]");
        migrationBuilder.Sql("DELETE [dbo].[Language]");
    }
}
