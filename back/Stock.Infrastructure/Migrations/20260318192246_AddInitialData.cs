using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialData : Migration
    {        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string scriptsPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Scripts", "InitialData");

            string sqlBrandData = File.ReadAllText(Path.Combine(scriptsPath, "BrandData.sql"));
            string sqlCategoryData = File.ReadAllText(Path.Combine(scriptsPath, "CategoryData.sql"));

            migrationBuilder.Sql(sqlBrandData);
            migrationBuilder.Sql(sqlCategoryData);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [dbo].[Storage]");
            migrationBuilder.Sql("DELETE [dbo].[Item]");
            migrationBuilder.Sql("DELETE [dbo].[Box]");
            migrationBuilder.Sql("DELETE [dbo].[Category]");
            migrationBuilder.Sql("DELETE [dbo].[Brand]");
        }
    }
}
