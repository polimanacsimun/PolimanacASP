using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.Repositories.Mock
{
    public class ProductMockRepository
    {
        private readonly List<Product> _products;

        public ProductMockRepository()
        {
            var electronics = new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices and equipment"
            };

            var office = new Category
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

            var alphaSupply = new Supplier
            {
                Id = 1,
                Name = "Alpha Supply",
                Address = "123 Main St",
                Phone = "+385 1 222 3333",
                Email = "sales@alpha.com",
                ContactPerson = "Ana Kovač",
                RegistrationDate = new System.DateTime(2024, 5, 1),
                IsActive = true
            };

            var betaTrade = new Supplier
            {
                Id = 2,
                Name = "Beta Trade",
                Address = "45 Commerce Rd",
                Phone = "+385 1 444 5555",
                Email = "info@beta.com",
                ContactPerson = "Marko Horvat",
                RegistrationDate = new System.DateTime(2023, 11, 10),
                IsActive = true
            };

            var laptop = new Product
            {
                Id = 1,
                Name = "Business Laptop",
                Description = "15-inch laptop with i7 processor and 16GB RAM",
                Price = 950m,
                UnitOfMeasure = "piece",
                MinimumStock = 5,
                CreatedAt = new System.DateTime(2025, 1, 10),
                IsActive = true,
                Type = ProductType.Physical,
                Category = electronics,
                Supplier = alphaSupply
            };

            var printerPaper = new Product
            {
                Id = 2,
                Name = "Printer Paper A4",
                Description = "500-sheet ream of A4 paper",
                Price = 14.5m,
                UnitOfMeasure = "pack",
                MinimumStock = 20,
                CreatedAt = new System.DateTime(2025, 1, 15),
                IsActive = true,
                Type = ProductType.Physical,
                Category = office,
                Supplier = betaTrade
            };

            var cloudLicense = new Product
            {
                Id = 3,
                Name = "Cloud Storage License",
                Description = "1-year subscription for 1TB storage",
                Price = 79m,
                UnitOfMeasure = "license",
                MinimumStock = 0,
                CreatedAt = new System.DateTime(2025, 2, 5),
                IsActive = true,
                Type = ProductType.Digital,
                Category = consumables,
                Supplier = betaTrade
            };

            electronics.Products.Add(laptop);
            office.Products.Add(printerPaper);
            consumables.Products.Add(cloudLicense);

            alphaSupply.Products.Add(laptop);
            betaTrade.Products.Add(printerPaper);
            betaTrade.Products.Add(cloudLicense);

            _products = new List<Product>
            {
                laptop,
                printerPaper,
                cloudLicense
            };
        }

        public List<Product> GetAll()
        {
            return _products.ToList();
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
