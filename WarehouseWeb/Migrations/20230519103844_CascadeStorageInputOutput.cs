using Microsoft.EntityFrameworkCore.Migrations;

namespace WarehouseWeb.Migrations
{
    public partial class CascadeStorageInputOutput : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput",
                column: "StorageItemId",
                principalTable: "StorageItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput",
                column: "StorageItemId",
                principalTable: "StorageItem",
                principalColumn: "Id");
        }
    }
}
