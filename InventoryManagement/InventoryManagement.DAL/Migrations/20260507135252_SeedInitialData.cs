using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Electronic devices and equipment", "Electronics" },
                    { 2, "Office supplies and stationery", "Office Supplies" },
                    { 3, "Consumable items and disposables", "Consumables" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "ContactPerson", "Email", "IsActive", "Name", "Phone", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, "123 Main St, Seattle, WA", "John Smith", "contact@alphasupply.com", true, "Alpha Supply", "+1-206-555-1000", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "456 Oak Ave, Portland, OR", "Jane Doe", "sales@betatrade.com", true, "Beta Trade", "+1-503-555-2000", new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsActive", "LastName", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "toni.peric@company.com", "Toni", true, "Perić", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "maja.babic@company.com", "Maja", true, "Babić", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 3, "ivan.ridic@company.com", "Ivan", true, "Riđić", new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Address", "Capacity", "Email", "IsActive", "Manager", "Name", "OpeningDate", "Phone", "Type" },
                values: new object[,]
                {
                    { 1, "789 Industrial Blvd, Seattle, WA", 50000, "warehouse@company.com", true, "Robert Johnson", "Main Warehouse", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1-206-555-3000", 0 },
                    { 2, "321 Logistics Way, Portland, OR", 25000, "regional@company.com", true, "Sarah Wilson", "Regional Warehouse", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1-503-555-4000", 1 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "DeliveryDate", "Note", "OrderDate", "OrderNumber", "Status", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 1, null, "Urgent delivery requested", new DateTime(2026, 4, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), "ORD-2026-001", 1, 2850m, 1 },
                    { 2, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delivered successfully", new DateTime(2026, 3, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), "ORD-2026-002", 2, 145m, 2 },
                    { 3, null, "Awaiting payment confirmation", new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Unspecified), "ORD-2026-003", 0, 948m, 3 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "MinimumStock", "Name", "Price", "SupplierId", "Type", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-performance laptop for business use", true, 5, "Business Laptop", 950m, 1, 0, "unit" },
                    { 2, 2, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standard A4 printer paper, 500 sheets per ream", true, 50, "Printer Paper A4", 14.5m, 2, 0, "ream" },
                    { 3, 3, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual cloud storage subscription, 100GB capacity", true, 0, "Cloud Storage License", 79m, 2, 1, "license" }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "LastCheckedAt", "MaximumQuantity", "MinimumQuantity", "ProductId", "QuantityInStock", "ShelfLocation", "WarehouseId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 5, 1, 45, "A-01-01", 1 },
                    { 2, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 500, 50, 2, 320, "B-02-03", 1 },
                    { 3, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, 0, 3, 150, "C-03-05", 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "Discount", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), 0m, 1, 1, 3, 2850m, 950m },
                    { 2, new DateTime(2026, 3, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2, 2, 10, 145m, 14.5m },
                    { 3, new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Unspecified), 0m, 3, 3, 12, 948m, 79m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
