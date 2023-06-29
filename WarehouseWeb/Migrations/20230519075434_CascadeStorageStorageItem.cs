using Microsoft.EntityFrameworkCore.Migrations;

namespace WarehouseWeb.Migrations
{
    public partial class CascadeStorageStorageItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "Id");
        }
    }
}
