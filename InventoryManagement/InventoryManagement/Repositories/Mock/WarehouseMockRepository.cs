using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Repositories.Mock
{
    public class WarehouseMockRepository
    {
        private readonly List<Warehouse> _warehouses;

        public WarehouseMockRepository()
        {
            var mainWarehouse = new Warehouse
            {
                Id = 1,
                Name = "Main Warehouse",
                Address = "123 Industrial Blvd, Seattle, WA 98101",
                Capacity = 50000,
                Phone = "+1 (206) 555-0100",
                Email = "main.warehouse@inventory.com",
                Manager = "Sarah Johnson",
                OpeningDate = new DateTime(2015, 3, 15),
                IsActive = true,
                Type = WarehouseType.Main,
                InventoryItems = new List<InventoryItem>()
            };

            var regionalWarehouse = new Warehouse
            {
                Id = 2,
                Name = "Regional Warehouse",
                Address = "456 Commerce Drive, Portland, OR 97201",
                Capacity = 25000,
                Phone = "+1 (503) 555-0200",
                Email = "regional.warehouse@inventory.com",
                Manager = "Michael Chen",
                OpeningDate = new DateTime(2018, 7, 22),
                IsActive = true,
                Type = WarehouseType.Regional,
                InventoryItems = new List<InventoryItem>()
            };

            // Create mock products for inventory references
            var businessLaptop = new Product
            {
                Id = 1,
                Name = "Business Laptop",
                Description = "15-inch laptop with i7 processor and 16GB RAM",
                Price = 950m,
                UnitOfMeasure = "piece",
                MinimumStock = 5,
                CreatedAt = new DateTime(2025, 1, 10),
                IsActive = true,
                Type = ProductType.Physical
            };

            var printerPaper = new Product
            {
                Id = 2,
                Name = "Printer Paper A4",
                Description = "500-sheet ream of A4 paper",
                Price = 14.5m,
                UnitOfMeasure = "pack",
                MinimumStock = 20,
                CreatedAt = new DateTime(2025, 1, 15),
                IsActive = true,
                Type = ProductType.Physical
            };

            var cloudLicense = new Product
            {
                Id = 3,
                Name = "Cloud Storage License",
                Description = "1-year subscription for 1TB storage",
                Price = 79m,
                UnitOfMeasure = "license",
                MinimumStock = 0,
                CreatedAt = new DateTime(2025, 2, 5),
                IsActive = true,
                Type = ProductType.Digital
            };

            // Inventory items for Main Warehouse
            mainWarehouse.InventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    QuantityInStock = 45,
                    MinimumQuantity = 5,
                    MaximumQuantity = 100,
                    ShelfLocation = "A-12-03",
                    LastCheckedAt = new DateTime(2026, 4, 14, 14, 30, 0),
                    Product = businessLaptop,
                    Warehouse = mainWarehouse
                },
                new InventoryItem
                {
                    Id = 2,
                    QuantityInStock = 320,
                    MinimumQuantity = 20,
                    MaximumQuantity = 500,
                    ShelfLocation = "B-05-01",
                    LastCheckedAt = new DateTime(2026, 4, 15, 09, 15, 0),
                    Product = printerPaper,
                    Warehouse = mainWarehouse
                }
            };

            // Inventory items for Regional Warehouse
            regionalWarehouse.InventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 3,
                    QuantityInStock = 150,
                    MinimumQuantity = 0,
                    MaximumQuantity = 300,
                    ShelfLocation = "C-08-02",
                    LastCheckedAt = new DateTime(2026, 4, 13, 11, 45, 0),
                    Product = cloudLicense,
                    Warehouse = regionalWarehouse
                }
            };

            _warehouses = new List<Warehouse>
            {
                mainWarehouse,
                regionalWarehouse
            };
        }

        public List<Warehouse> GetAll()
        {
            return _warehouses.ToList();
        }

        public Warehouse? GetById(int id)
        {
            return _warehouses.FirstOrDefault(w => w.Id == id);
        }
    }
}
