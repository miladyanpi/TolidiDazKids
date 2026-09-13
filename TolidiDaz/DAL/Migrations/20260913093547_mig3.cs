using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Customer_CustomerID1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomerID1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerID1",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerID1",
                table: "AspNetUsers",
                column: "CustomerID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Customer_CustomerID1",
                table: "AspNetUsers",
                column: "CustomerID1",
                principalTable: "Customer",
                principalColumn: "ID");
        }
    }
}
