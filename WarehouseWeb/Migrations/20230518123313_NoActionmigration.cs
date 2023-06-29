using Microsoft.EntityFrameworkCore.Migrations;

namespace WarehouseWeb.Migrations
{
    public partial class NoActionmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Product_ProductId",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput",
                column: "StorageItemId",
                principalTable: "StorageItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Product_ProductId",
                table: "StorageItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Product_ProductId",
                table: "StorageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageInputOutput_StorageItem_StorageItemId",
                table: "StorageInputOutput",
                column: "StorageItemId",
                principalTable: "StorageItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Product_ProductId",
                table: "StorageItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Storage_StorageId",
                table: "StorageItem",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
