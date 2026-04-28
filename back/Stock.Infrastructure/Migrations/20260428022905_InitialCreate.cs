using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Background = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IncludedIn = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IncludedIn = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Context = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LabelKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.LabelId);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("Relational:DefaultConstraintName", "DF_Language_IsDefault")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "Box",
                columns: table => new
                {
                    BoxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentBoxId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Depth = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(15,2)", nullable: false, computedColumnSql: "ISNULL(CAST(([Height] * [Width] * [Depth]) AS decimal(15, 2)), 0.0)", stored: true),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Box_CreatedAt"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Box_UpdatedAt")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Box", x => x.BoxId);
                    table.CheckConstraint("CK_Box_Depth", "[Depth] > 0");
                    table.CheckConstraint("CK_Box_Height", "[Height] > 0");
                    table.CheckConstraint("CK_Box_Width", "[Width] > 0");
                    table.ForeignKey(
                        name: "FK_Box_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Box_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Box_ParentBoxId",
                        column: x => x.ParentBoxId,
                        principalTable: "Box",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Item_CreatedAt"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Item_UpdatedAt")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Item_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    TranslationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    LabelId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Translation_CreatedAt"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Translation_UpdatedAt")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.TranslationId);
                    table.ForeignKey(
                        name: "FK_Translation_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Label",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translation_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Expires = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresOn = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Storage_CreatedAt"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                        .Annotation("Relational:DefaultConstraintName", "DF_Storage_UpdatedAt")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.StorageId);
                    table.CheckConstraint("CK_Storage_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_Storage_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Box",
                        principalColumn: "BoxId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Storage_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Storage_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Box_BrandId",
                table: "Box",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Box_CategoryId",
                table: "Box",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ_Box_Name",
                table: "Box",
                columns: new[] { "ParentBoxId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Brand_Name",
                table: "Brand",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryId",
                table: "Item",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ_Item_Name",
                table: "Item",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Label_Context_Key",
                table: "Label",
                columns: new[] { "Context", "LabelKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Language_SingleDefault",
                table: "Language",
                column: "IsDefault",
                unique: true,
                filter: "[IsDefault] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_BrandId",
                table: "Storage",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemId",
                table: "Storage",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "UQ_Storage_Box_Item_Brand",
                table: "Storage",
                columns: new[] { "BoxId", "ItemId", "BrandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_LanguageCode",
                table: "Translation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "UQ_Translation_Label_Language",
                table: "Translation",
                columns: new[] { "LabelId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "Box");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
