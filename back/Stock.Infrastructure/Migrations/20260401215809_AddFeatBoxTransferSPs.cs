using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatBoxTransferSPs : Migration
    {        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

            string sqlGetAvailableParentBoxes = File.ReadAllText(Path.Combine(scriptsPath, "GetAvailableParentBoxes.sql"));
            string sqlMoveBox = File.ReadAllText(Path.Combine(scriptsPath, "MoveBox.sql"));
            string sqlGetBoxFullPath = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxFullPath.sql"));

            migrationBuilder.Sql(sqlGetAvailableParentBoxes);
            migrationBuilder.Sql(sqlMoveBox);
            migrationBuilder.Sql(sqlGetBoxFullPath);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAvailableParentBoxes");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS MoveBox");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBoxFullPath");
        }
    }
}
