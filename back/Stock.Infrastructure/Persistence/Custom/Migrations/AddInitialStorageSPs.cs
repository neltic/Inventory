using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialStorageSPs
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlRemoveItemFromBox = File.ReadAllText(Path.Combine(scriptsPath, "RemoveItemFromBox.sql"));
        string sqlAddOrEditStorage = File.ReadAllText(Path.Combine(scriptsPath, "AddOrEditStorage.sql"));
        string sqlGetStorageByItemId = File.ReadAllText(Path.Combine(scriptsPath, "GetStorageByItemId.sql"));

        migrationBuilder.Sql(sqlRemoveItemFromBox);
        migrationBuilder.Sql(sqlAddOrEditStorage);
        migrationBuilder.Sql(sqlGetStorageByItemId);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.RemoveItemFromBox");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.AddOrEditStorage");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetStorageByItemId");
    }
}
