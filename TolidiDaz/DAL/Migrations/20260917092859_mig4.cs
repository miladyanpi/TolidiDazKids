using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SkuCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trait",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trait", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTrait",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    TraitID = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTrait", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CategoryTrait_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTrait_Trait_TraitID",
                        column: x => x.TraitID,
                        principalTable: "Trait",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraitValue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TraitID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayColorHex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraitValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraitValue_Trait_TraitID",
                        column: x => x.TraitID,
                        principalTable: "Trait",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantValue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantID = table.Column<int>(type: "int", nullable: false),
                    TraitValueID = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductVariantValue_ProductVariant_ProductVariantID",
                        column: x => x.ProductVariantID,
                        principalTable: "ProductVariant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantValue_TraitValue_TraitValueID",
                        column: x => x.TraitValueID,
                        principalTable: "TraitValue",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrait_CategoryID",
                table: "CategoryTrait",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrait_TraitID",
                table: "CategoryTrait",
                column: "TraitID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductID",
                table: "ProductVariant",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValue_ProductVariantID",
                table: "ProductVariantValue",
                column: "ProductVariantID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValue_TraitValueID",
                table: "ProductVariantValue",
                column: "TraitValueID");

            migrationBuilder.CreateIndex(
                name: "IX_TraitValue_TraitID",
                table: "TraitValue",
                column: "TraitID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTrait");

            migrationBuilder.DropTable(
                name: "ProductVariantValue");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropTable(
                name: "TraitValue");

            migrationBuilder.DropTable(
                name: "Trait");
        }
    }
}
