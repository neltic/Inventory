using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialStorageSPs : Migration
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
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UnbindBoxAndRefresh");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddOrEditStorage");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetStorageByItemId");
        }
    }
}
