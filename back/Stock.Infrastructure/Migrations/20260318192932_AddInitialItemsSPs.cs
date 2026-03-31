using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialItemsSPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

            string sqlGetItemById = File.ReadAllText(Path.Combine(scriptsPath, "GetItemById.sql"));

            migrationBuilder.Sql(sqlGetItemById);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetItemById");
        }
    }
}
