using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TraitValue",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "ProductVariant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleEndsDate",
                table: "ProductVariant",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalePrice",
                table: "ProductVariant",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleStartsDate",
                table: "ProductVariant",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TraitValue_Code",
                table: "TraitValue",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TraitValue_Code",
                table: "TraitValue");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "SaleEndsDate",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "SaleStartsDate",
                table: "ProductVariant");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TraitValue",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
