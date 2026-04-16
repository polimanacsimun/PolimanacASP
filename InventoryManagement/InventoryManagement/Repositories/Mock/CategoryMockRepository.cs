using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Repositories.Mock
{
    public class CategoryMockRepository
    {
        private readonly List<Category> _categories;

        public CategoryMockRepository()
        {
            var electronics = new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices and equipment"
            };

            var officeSupplies = new Category
            {
                Id = 2,
                Name = "Office Supplies",
                Description = "Office materials and consumables"
            };

            var consumables = new Category
            {
                Id = 3,
                Name = "Consumables",
                Description = "Digital and recurring service products"
            };

            // Connect products to categories
            electronics.Products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Business Laptop",
                    Description = "15-inch laptop with i7 processor and 16GB RAM",
                    Price = 950m,
                    UnitOfMeasure = "piece",
                    MinimumStock = 5,
                    CreatedAt = new DateTime(2025, 1, 10),
                    IsActive = true,
                    Category = electronics,
                    Type = ProductType.Physical
                }
            };

            officeSupplies.Products = new List<Product>
            {
                new Product
                {
                    Id = 2,
                    Name = "Printer Paper A4",
                    Description = "500-sheet ream of A4 paper",
                    Price = 14.5m,
                    UnitOfMeasure = "pack",
                    MinimumStock = 20,
                    CreatedAt = new DateTime(2025, 1, 15),
                    IsActive = true,
                    Category = officeSupplies,
                    Type = ProductType.Physical
                }
            };

            consumables.Products = new List<Product>
            {
                new Product
                {
                    Id = 3,
                    Name = "Cloud Storage License",
                    Description = "1-year subscription for 1TB storage",
                    Price = 79m,
                    UnitOfMeasure = "license",
                    MinimumStock = 0,
                    CreatedAt = new DateTime(2025, 2, 5),
                    IsActive = true,
                    Category = consumables,
                    Type = ProductType.Digital
                }
            };

            _categories = new List<Category>
            {
                electronics,
                officeSupplies,
                consumables
            };
        }

        public List<Category> GetAll()
        {
            return _categories.ToList();
        }

        public Category? GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }
    }
}
