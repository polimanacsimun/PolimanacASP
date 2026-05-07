---
name: entity-framework-skill
description: Use this skill when modifying EF-ready domain models, configuring DbContext, adding DbSet properties, creating EF repositories, generating migrations, updating connection strings, or working with database persistence in the InventoryManagement ASP.NET Core MVC app.
---

# Entity Framework Skill

Use this skill for Entity Framework Core work in the InventoryManagement project.

## Project context

The application is an ASP.NET Core MVC app for inventory management.

The current solution contains:
- `InventoryManagement` — MVC web project
- `InventoryManagement.Domain` — domain models and enums
- optional `InventoryManagement.DAL` — data access layer for EF Core and migrations

Main domain entities:
- Product
- Category
- Supplier
- Warehouse
- InventoryItem
- User
- Order
- OrderItem

The application originally used mock repositories in Lab 2. In Lab 3, the application should be moved toward Entity Framework repositories and database persistence.

## EF model rules

When modifying domain model classes:

1. Add `[Key]` to the `Id` property of each entity.
2. Use `System.ComponentModel.DataAnnotations`.
3. Use `System.ComponentModel.DataAnnotations.Schema` for `[ForeignKey]`.
4. Navigation reference properties should be `virtual`.
5. Collection navigation properties should be `virtual ICollection<T>`, not `List<T>`.
6. Initialize collection navigation properties in the constructor or inline using `new List<T>()` or `new HashSet<T>()`.
7. For one-to-many relationships, include both:
   - foreign key property, for example `CategoryId`
   - navigation property, for example `virtual Category Category`
8. Use `[ForeignKey(nameof(Category))]` on the FK property when clarity is useful.
9. Do not remove existing business properties unless explicitly asked.
10. Do not introduce Create/Edit/Delete UI unless explicitly asked.

## Relationship expectations

Expected relationships:

- Category 1-N Product
- Supplier 1-N Product
- Product N-N Warehouse through InventoryItem
- Warehouse 1-N InventoryItem
- Product 1-N InventoryItem
- User 1-N Order
- Order N-N Product through OrderItem
- Order 1-N OrderItem
- Product 1-N OrderItem

InventoryItem is a junction/entity class between Product and Warehouse.

OrderItem is a junction/entity class between Order and Product.

## DbContext rules

When creating or editing the EF DbContext:

1. The DbContext should be named `InventoryManagementDbContext`.
2. It should inherit from `Microsoft.EntityFrameworkCore.DbContext`.
3. It should have a constructor accepting `DbContextOptions<InventoryManagementDbContext>`.
4. It should expose DbSet properties:
   - `DbSet<Product> Products`
   - `DbSet<Category> Categories`
   - `DbSet<Supplier> Suppliers`
   - `DbSet<Warehouse> Warehouses`
   - `DbSet<InventoryItem> InventoryItems`
   - `DbSet<User> Users`
   - `DbSet<Order> Orders`
   - `DbSet<OrderItem> OrderItems`
5. Use `OnModelCreating` only when annotations are not enough or when seed data/configuration is needed.

## Repository rules

When creating EF repositories:

1. Prefer constructor injection of `InventoryManagementDbContext`.
2. Use LINQ queries over DbSet properties.
3. Use `Include` and `ThenInclude` when related data is required by views.
4. Keep controller logic simple.
5. Repositories should provide at least:
   - `GetAll()`
   - `GetById(int id)`
6. Do not use mock data inside EF repositories.

## Migration rules

When generating migrations:

1. Ensure the project builds first.
2. Ensure the startup project is the MVC web project.
3. Ensure the project containing the DbContext is selected correctly.
4. Use descriptive migration names, for example:
   - `InitialCreate`
   - `AddInventoryRelations`
   - `SeedInitialData`
5. After creating a migration, run database update.

Typical commands if using a separate DAL project:

```bash
dotnet ef migrations add InitialCreate --project InventoryManagement.DAL --startup-project InventoryManagement --context InventoryManagementDbContext
dotnet ef database update --project InventoryManagement.DAL --startup-project InventoryManagement --context InventoryManagementDbContext