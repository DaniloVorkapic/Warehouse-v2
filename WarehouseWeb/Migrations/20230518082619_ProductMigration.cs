using Microsoft.EntityFrameworkCore.Migrations;

namespace WarehouseWeb.Migrations
{
    public partial class ProductMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItem_Supplier_SupplierId",
                table: "StorageItem");

            migrationBuilder.DropIndex(
                name: "IX_StorageItem_SupplierId",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "StorageItem");

            migrationBuilder.RenameColumn(
                name: "Quantity_ClassificationValuesId",
                table: "StorageItem",
                newName: "Quantity_MeasurementUnitId");

            migrationBuilder.RenameColumn(
                name: "Quantity_ClassificationValuesId",
                table: "StorageInputOutput",
                newName: "Quantity_MeasurementUnitId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Product",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Quantity_ClassificationValuesId",
                table: "OrderItem",
                newName: "Quantity_MeasurementUnitId");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Supplier",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Supplier",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "StorageItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "StorageItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "StorageInputOutput",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "StorageInputOutput",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Storage",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Storage",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "RoleClaims",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "RoleClaims",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Role",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Role",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "OrderItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "OrderItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "CustomerRole",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "CustomerRole",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Customer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Company",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Company",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "ClassificationValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "ClassificationValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "ClassificationSpecification",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "ClassificationSpecification",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Claims",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifyBy",
                table: "Claims",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierId",
                table: "Product",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "StorageItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StorageInputOutput");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "StorageInputOutput");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Storage");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CustomerRole");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "CustomerRole");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClassificationValues");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "ClassificationValues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClassificationSpecification");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "ClassificationSpecification");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "Quantity_MeasurementUnitId",
                table: "StorageItem",
                newName: "Quantity_ClassificationValuesId");

            migrationBuilder.RenameColumn(
                name: "Quantity_MeasurementUnitId",
                table: "StorageInputOutput",
                newName: "Quantity_ClassificationValuesId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Product",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Quantity_MeasurementUnitId",
                table: "OrderItem",
                newName: "Quantity_ClassificationValuesId");

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "StorageItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StorageItem_SupplierId",
                table: "StorageItem",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItem_Supplier_SupplierId",
                table: "StorageItem",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
