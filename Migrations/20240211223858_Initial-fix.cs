using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BethanysPieShopAdmin.Migrations
{
    /// <inheritdoc />
    public partial class Initialfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pies_C_CategoryId",
                table: "Pies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_C",
                table: "C");

            migrationBuilder.RenameTable(
                name: "C",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pies_Categories_CategoryId",
                table: "Pies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pies_Categories_CategoryId",
                table: "Pies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "C");

            migrationBuilder.AddPrimaryKey(
                name: "PK_C",
                table: "C",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pies_C_CategoryId",
                table: "Pies",
                column: "CategoryId",
                principalTable: "C",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
