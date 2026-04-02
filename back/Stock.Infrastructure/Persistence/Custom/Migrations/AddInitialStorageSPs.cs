using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialStorageSPs
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlUnbindBoxAndRefresh = File.ReadAllText(Path.Combine(scriptsPath, "UnbindBoxAndRefresh.sql"));
        string sqlAddOrEditStorage = File.ReadAllText(Path.Combine(scriptsPath, "AddOrEditStorage.sql"));
        string sqlGetStorageByItemId = File.ReadAllText(Path.Combine(scriptsPath, "GetStorageByItemId.sql"));

        migrationBuilder.Sql(sqlUnbindBoxAndRefresh);
        migrationBuilder.Sql(sqlAddOrEditStorage);
        migrationBuilder.Sql(sqlGetStorageByItemId);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.UnbindBoxAndRefresh");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.AddOrEditStorage");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetStorageByItemId");
    }
}
