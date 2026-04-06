using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddFeatBoxTransferSPs
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
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetAvailableParentBoxes");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.MoveBox");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetBoxFullPath");
    }
}
