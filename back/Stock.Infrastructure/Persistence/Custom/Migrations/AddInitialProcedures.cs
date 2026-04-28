using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.Infrastructure.Migrations;

public partial class AddInitialProcedures
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts");

        string sqlGetBoxById = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxById.sql"));
        string sqlGetBoxesByParent = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxesByParent.sql"));
        string sqlGetBoxFullPathByParent = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxFullPathByParent.sql"));
        string sqlGetBoxesLookup = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxesLookup.sql"));
        string sqlGetItemById = File.ReadAllText(Path.Combine(scriptsPath, "GetItemById.sql"));
        string sqlGetItemLocation = File.ReadAllText(Path.Combine(scriptsPath, "GetItemLocation.sql"));
        string sqlRemoveItemFromBox = File.ReadAllText(Path.Combine(scriptsPath, "RemoveItemFromBox.sql"));
        string sqlAddOrEditStorage = File.ReadAllText(Path.Combine(scriptsPath, "AddOrEditStorage.sql"));
        string sqlGetStorageByItemId = File.ReadAllText(Path.Combine(scriptsPath, "GetStorageByItemId.sql"));
        string sqlOrganizeCategories = File.ReadAllText(Path.Combine(scriptsPath, "OrganizeCategories.sql"));
        string sqlDeleteCategory = File.ReadAllText(Path.Combine(scriptsPath, "DeleteCategory.sql"));
        string sqlReorderCategory = File.ReadAllText(Path.Combine(scriptsPath, "ReorderCategory.sql"));
        string sqlGetAllTranslations = File.ReadAllText(Path.Combine(scriptsPath, "GetAllTranslations.sql"));
        string sqlGetAvailableParentBoxes = File.ReadAllText(Path.Combine(scriptsPath, "GetAvailableParentBoxes.sql"));
        string sqlMoveBox = File.ReadAllText(Path.Combine(scriptsPath, "MoveBox.sql"));
        string sqlGetBoxFullPath = File.ReadAllText(Path.Combine(scriptsPath, "GetBoxFullPath.sql"));

        migrationBuilder.Sql(sqlGetBoxById);
        migrationBuilder.Sql(sqlGetBoxesByParent);
        migrationBuilder.Sql(sqlGetBoxFullPathByParent);
        migrationBuilder.Sql(sqlGetBoxesLookup);
        migrationBuilder.Sql(sqlGetItemById);
        migrationBuilder.Sql(sqlGetItemLocation);
        migrationBuilder.Sql(sqlRemoveItemFromBox);
        migrationBuilder.Sql(sqlAddOrEditStorage);
        migrationBuilder.Sql(sqlGetStorageByItemId);
        migrationBuilder.Sql(sqlOrganizeCategories);
        migrationBuilder.Sql(sqlDeleteCategory);
        migrationBuilder.Sql(sqlReorderCategory);
        migrationBuilder.Sql(sqlGetAllTranslations);
        migrationBuilder.Sql(sqlGetAvailableParentBoxes);
        migrationBuilder.Sql(sqlMoveBox);
        migrationBuilder.Sql(sqlGetBoxFullPath);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBoxById]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBoxesByParent]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBoxFullPathByParent]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBoxesLookup]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetItemById]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetItemLocation]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[RemoveItemFromBox]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[AddOrEditStorage]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetStorageByItemId]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[OrganizeCategories]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteCategory]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ReorderCategory]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllTranslations]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAvailableParentBoxes]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[MoveBox]");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetBoxFullPath]");
    }
}
