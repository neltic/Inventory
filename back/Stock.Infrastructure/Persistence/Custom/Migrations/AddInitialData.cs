using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialData
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts", "InitialData");

        string sqlBrandData = File.ReadAllText(Path.Combine(scriptsPath, "BrandData.sql"));
        string sqlCategoryData = File.ReadAllText(Path.Combine(scriptsPath, "CategoryData.sql"));
        string sqlGlobalizationData = File.ReadAllText(Path.Combine(scriptsPath, "GlobalizationData.sql"));

        migrationBuilder.Sql(sqlBrandData);
        migrationBuilder.Sql(sqlCategoryData);
        migrationBuilder.Sql(sqlGlobalizationData);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE [dbo].[Storage]");
        migrationBuilder.Sql("DELETE [dbo].[Item]");
        migrationBuilder.Sql("DELETE [dbo].[Box]");
        migrationBuilder.Sql("DELETE [dbo].[Category]");
        migrationBuilder.Sql("DELETE [dbo].[Brand]");
        migrationBuilder.Sql("DELETE [dbo].[Translation]");
        migrationBuilder.Sql("DELETE [dbo].[Label]");
        migrationBuilder.Sql("DELETE [dbo].[Language]");
    }
}
