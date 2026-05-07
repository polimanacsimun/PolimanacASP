# Semantic Database Model

## Project

The application is an ASP.NET Core MVC inventory management system.  
The domain model represents products, categories, suppliers, warehouses, users, orders, order items, and inventory items.

The application was originally based on mock repositories, but in Lab 3 the domain model was prepared for Entity Framework Core and mapped to a relational database.

---

## Entity Overview

| Entity / Table | Purpose |
|---|---|
| Product | Represents an inventory product or catalog item. |
| Category | Groups products into business categories. |
| Supplier | Represents a vendor or supplier that provides products. |
| Warehouse | Represents a physical storage location. |
| InventoryItem | Junction entity between Product and Warehouse. Tracks stock quantities per warehouse. |
| User | Represents a system user or customer. |
| Order | Represents a product order placed by a user. |
| OrderItem | Junction entity between Order and Product. Represents a product line inside an order. |

---

## Product

Represents a product in the inventory catalog.

### Main properties

- Id
- Name
- Description
- Price
- UnitOfMeasure
- MinimumStock
- CreatedAt
- IsActive
- Type
- CategoryId
- SupplierId

### Relationships

- Many products belong to one category.
- Many products belong to one supplier.
- One product can appear in many inventory items.
- One product can appear in many order items.

### EF notes

- `Id` is the primary key.
- `CategoryId` is a foreign key to Category.
- `SupplierId` is a foreign key to Supplier.
- Navigation properties are virtual.
- Collection navigation properties use `ICollection<T>`.

---

## Category

Represents a product category.

### Main properties

- Id
- Name
- Description

### Relationships

- One category can contain many products.

### EF notes

- `Id` is the primary key.
- `Products` is a virtual `ICollection<Product>`.

---

## Supplier

Represents a supplier or vendor.

### Main properties

- Id
- Name
- Address
- Phone
- Email
- ContactPerson
- RegistrationDate
- IsActive

### Relationships

- One supplier can provide many products.

### EF notes

- `Id` is the primary key.
- `Products` is a virtual `ICollection<Product>`.

---

## Warehouse

Represents a physical warehouse.

### Main properties

- Id
- Name
- Address
- Capacity
- Phone
- Email
- Manager
- OpeningDate
- IsActive
- Type

### Relationships

- One warehouse can contain many inventory items.
- Products are connected to warehouses through InventoryItem.

### EF notes

- `Id` is the primary key.
- `InventoryItems` is a virtual `ICollection<InventoryItem>`.

---

## InventoryItem

Represents stock of a specific product in a specific warehouse.

### Main properties

- Id
- QuantityInStock
- MinimumQuantity
- MaximumQuantity
- ShelfLocation
- LastCheckedAt
- ProductId
- WarehouseId

### Relationships

- Many inventory items belong to one product.
- Many inventory items belong to one warehouse.
- InventoryItem acts as a junction entity between Product and Warehouse.

### EF notes

- `Id` is the primary key.
- `ProductId` is a foreign key to Product.
- `WarehouseId` is a foreign key to Warehouse.
- The combination of ProductId and WarehouseId is unique.

---

## User

Represents a system user.

### Main properties

- Id
- FirstName
- LastName
- Email
- Role
- RegistrationDate
- IsActive

### Relationships

- One user can have many orders.

### EF notes

- `Id` is the primary key.
- `Orders` is a virtual `ICollection<Order>`.

---

## Order

Represents an order placed by a user.

### Main properties

- Id
- OrderNumber
- OrderDate
- TotalPrice
- Status
- DeliveryDate
- Note
- UserId

### Relationships

- Many orders belong to one user.
- One order can contain many order items.
- Products are connected to orders through OrderItem.

### EF notes

- `Id` is the primary key.
- `UserId` is a foreign key to User.
- `OrderItems` is a virtual `ICollection<OrderItem>`.

---

## OrderItem

Represents one product line inside an order.

### Main properties

- Id
- Quantity
- UnitPrice
- TotalPrice
- Discount
- CreatedAt
- OrderId
- ProductId

### Relationships

- Many order items belong to one order.
- Many order items belong to one product.
- OrderItem acts as a junction entity between Order and Product.

### EF notes

- `Id` is the primary key.
- `OrderId` is a foreign key to Order.
- `ProductId` is a foreign key to Product.

---

## Relationship Summary

| Relationship | Type | Implementation |
|---|---|---|
| Category -> Product | 1-N | Product has CategoryId and Category navigation property. Category has Products collection. |
| Supplier -> Product | 1-N | Product has SupplierId and Supplier navigation property. Supplier has Products collection. |
| Product -> InventoryItem | 1-N | InventoryItem has ProductId. Product has InventoryItems collection. |
| Warehouse -> InventoryItem | 1-N | InventoryItem has WarehouseId. Warehouse has InventoryItems collection. |
| Product <-> Warehouse | N-N through junction entity | InventoryItem connects Product and Warehouse. |
| User -> Order | 1-N | Order has UserId and User navigation property. User has Orders collection. |
| Order -> OrderItem | 1-N | OrderItem has OrderId. Order has OrderItems collection. |
| Product -> OrderItem | 1-N | OrderItem has ProductId. Product has OrderItems collection. |
| Order <-> Product | N-N through junction entity | OrderItem connects Order and Product. |

---

## DbContext

The EF Core database context is named:

`InventoryManagementDbContext`

It contains DbSet properties for:

- Products
- Categories
- Suppliers
- Warehouses
- InventoryItems
- Users
- Orders
- OrderItems

The DbContext is registered in `Program.cs` using dependency injection and SQL Server configuration.

---

## Migration

The initial database schema was generated using Entity Framework Core migrations.

The initial migration creates tables based on the EF-ready domain model and maps relationships between entities using primary keys, foreign keys, navigation properties, and collection properties.