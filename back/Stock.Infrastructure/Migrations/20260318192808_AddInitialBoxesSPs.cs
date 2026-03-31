using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialBoxesSPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

            string sqlGetBoxById = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxById.sql"));
            string sqlGetBoxesByParent = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxesByParent.sql"));
            string sqlGetBoxFullPathByParent = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxFullPathByParent.sql"));
            string sqlGetBoxesLookup = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxesLookup.sql"));

            migrationBuilder.Sql(sqlGetBoxById);
            migrationBuilder.Sql(sqlGetBoxesByParent);
            migrationBuilder.Sql(sqlGetBoxFullPathByParent);
            migrationBuilder.Sql(sqlGetBoxesLookup);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBoxById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBoxesByParent");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBoxFullPathByParent");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBoxesLookup");
        }
    }
}
