using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialGlobalizationSPs
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlGetAllTranslations = File.ReadAllText(Path.Combine(scriptsPath, "GetAllTranslations.sql"));
        
        migrationBuilder.Sql(sqlGetAllTranslations);        
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllTranslations]");
    }
}
