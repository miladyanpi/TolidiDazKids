using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariantValue_ProductVariantID",
                table: "ProductVariantValue");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTrait_CategoryID",
                table: "CategoryTrait");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SkuCode",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "ProductVariant",
                newName: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TraitValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SkuCode",
                table: "ProductVariant",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PricingType",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BasePrice",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseStock",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValue_ProductVariantID_TraitValueID",
                table: "ProductVariantValue",
                columns: new[] { "ProductVariantID", "TraitValueID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_SkuCode",
                table: "ProductVariant",
                column: "SkuCode",
                unique: true,
                filter: "[SkuCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrait_CategoryID_TraitID",
                table: "CategoryTrait",
                columns: new[] { "CategoryID", "TraitID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariantValue_ProductVariantID_TraitValueID",
                table: "ProductVariantValue");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_SkuCode",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTrait_CategoryID_TraitID",
                table: "CategoryTrait");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "TraitValue");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BaseStock",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "ProductVariant",
                newName: "Count");

            migrationBuilder.AlterColumn<string>(
                name: "SkuCode",
                table: "ProductVariant",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PricingType",
                table: "Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SkuCode",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValue_ProductVariantID",
                table: "ProductVariantValue",
                column: "ProductVariantID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrait_CategoryID",
                table: "CategoryTrait",
                column: "CategoryID");
        }
    }
}
