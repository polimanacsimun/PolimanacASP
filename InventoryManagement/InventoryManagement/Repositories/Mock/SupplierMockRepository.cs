using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.Repositories.Mock
{
    public class SupplierMockRepository
    {
        private readonly List<Supplier> _suppliers;

        public SupplierMockRepository()
        {
            var alphaSupply = new Supplier
            {
                Id = 1,
                Name = "Alpha Supply",
                Address = "123 Main St, Industrial Zone 1",
                Phone = "+385 1 222 3333",
                Email = "sales@alpha.com",
                ContactPerson = "Ana Kovač",
                RegistrationDate = new DateTime(2024, 5, 1),
                IsActive = true
            };

            var betaTrade = new Supplier
            {
                Id = 2,
                Name = "Beta Trade",
                Address = "45 Commerce Rd, Logistics Park 2",
                Phone = "+385 1 444 5555",
                Email = "info@beta.com",
                ContactPerson = "Marko Horvat",
                RegistrationDate = new DateTime(2023, 11, 10),
                IsActive = true
            };

            // Note: Products are referenced here but would be populated 
            // from ProductMockRepository in a real scenario            
            alphaSupply.Products = new List<Product>
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
                    Supplier = alphaSupply
                }
            };

            betaTrade.Products = new List<Product>
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
                    Supplier = betaTrade
                },
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
                    Supplier = betaTrade
                }
            };

            _suppliers = new List<Supplier>
            {
                alphaSupply,
                betaTrade
            };
        }

        public List<Supplier> GetAll()
        {
            return _suppliers.ToList();
        }

        public Supplier? GetById(int id)
        {
            return _suppliers.FirstOrDefault(s => s.Id == id);
        }
    }
}
