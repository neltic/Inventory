using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialItemsSPs
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlGetItemById = File.ReadAllText(Path.Combine(scriptsPath, "GetItemById.sql"));
        string sqlGetItemLocation = File.ReadAllText(Path.Combine(scriptsPath, "GetItemLocation.sql"));

        migrationBuilder.Sql(sqlGetItemById);
        migrationBuilder.Sql(sqlGetItemLocation);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetItemById");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetItemLocation");
    }
}
