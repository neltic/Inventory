using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialCategoriesSPs
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlOrganizeCategories = File.ReadAllText(Path.Combine(scriptsPath, "OrganizeCategories.sql"));
        string sqlDeleteCategory = File.ReadAllText(Path.Combine(scriptsPath, "DeleteCategory.sql"));
        string sqlReorderCategory = File.ReadAllText(Path.Combine(scriptsPath, "ReorderCategory.sql"));

        migrationBuilder.Sql(sqlOrganizeCategories);
        migrationBuilder.Sql(sqlDeleteCategory);
        migrationBuilder.Sql(sqlReorderCategory);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.OrganizeCategories");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.DeleteCategory");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.ReorderCategory");
    }
}
