using System.Net;
using System.Net.Http.Json;
using InventoryManagement.Domain.Enums;
using InventoryManagement.DTOs;

namespace InventoryManagement.Tests;

public class ApiCrudIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiCrudIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Category_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/categories?q=electronics");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/categories/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/categories", new CategoryRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/categories", new CategoryRequestDto
        {
            Name = "Integration Category",
            Description = "Created from integration test"
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/categories/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/categories/{created.Id}", new CategoryRequestDto
        {
            Name = "Integration Category Updated",
            Description = "Updated from integration test"
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/categories/99999", new CategoryRequestDto
        {
            Name = "Missing Category",
            Description = "Should not update anything"
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/categories/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/categories/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Supplier_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/suppliers?q=alpha");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/suppliers/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/suppliers", new SupplierRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/suppliers", new SupplierRequestDto
        {
            Name = "Integration Supplier",
            Address = "Test Street 1",
            Phone = "+385-1-111-222",
            Email = "integration.supplier@example.com",
            ContactPerson = "Test Person",
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<SupplierDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/suppliers/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/suppliers/{created.Id}", new SupplierRequestDto
        {
            Name = "Integration Supplier Updated",
            Address = "Updated Street 2",
            Phone = "+385-1-333-444",
            Email = "integration.supplier.updated@example.com",
            ContactPerson = "Updated Person",
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/suppliers/99999", new SupplierRequestDto
        {
            Name = "Missing Supplier",
            Address = "Nowhere",
            Phone = "+385-1-000-000",
            Email = "missing.supplier@example.com",
            ContactPerson = "Nobody",
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/suppliers/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/suppliers/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Warehouse_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/warehouses?q=main");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/warehouses/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/warehouses", new WarehouseRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/warehouses", new WarehouseRequestDto
        {
            Name = "Integration Warehouse",
            Address = "Warehouse Street 1",
            Capacity = 1000,
            Phone = "+385-1-555-666",
            Email = "warehouse.integration@example.com",
            Manager = "Warehouse Manager",
            OpeningDate = DateTime.UtcNow.Date,
            IsActive = true,
            Type = WarehouseType.Distribution
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<WarehouseDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/warehouses/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/warehouses/{created.Id}", new WarehouseRequestDto
        {
            Name = "Integration Warehouse Updated",
            Address = "Warehouse Street 2",
            Capacity = 2000,
            Phone = "+385-1-777-888",
            Email = "warehouse.integration.updated@example.com",
            Manager = "Updated Manager",
            OpeningDate = DateTime.UtcNow.Date,
            IsActive = true,
            Type = WarehouseType.Regional
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/warehouses/99999", new WarehouseRequestDto
        {
            Name = "Missing Warehouse",
            Address = "Nowhere",
            Capacity = 100,
            Phone = "+385-1-000-000",
            Email = "missing.warehouse@example.com",
            Manager = "Nobody",
            OpeningDate = DateTime.UtcNow.Date,
            IsActive = true,
            Type = WarehouseType.Main
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/warehouses/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/warehouses/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Product_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/products?q=laptop");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/products/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/products", new ProductRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/products", new ProductRequestDto
        {
            Name = "Integration Product",
            Description = "Created from integration test",
            Price = 25.50m,
            UnitOfMeasure = "unit",
            MinimumStock = 3,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Type = ProductType.Physical,
            CategoryId = 1,
            SupplierId = 1
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/products/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/products/{created.Id}", new ProductRequestDto
        {
            Name = "Integration Product Updated",
            Description = "Updated from integration test",
            Price = 30.25m,
            UnitOfMeasure = "unit",
            MinimumStock = 5,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Type = ProductType.Service,
            CategoryId = 1,
            SupplierId = 1
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/products/99999", new ProductRequestDto
        {
            Name = "Missing Product",
            Description = "Should not update anything",
            Price = 10m,
            UnitOfMeasure = "unit",
            MinimumStock = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Type = ProductType.Physical,
            CategoryId = 1,
            SupplierId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/products/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/products/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Inventory_item_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var productResponse = await client.PostAsJsonAsync("/api/products", new ProductRequestDto
        {
            Name = "Inventory Test Product",
            Description = "Product for inventory item test",
            Price = 10m,
            UnitOfMeasure = "unit",
            MinimumStock = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Type = ProductType.Physical,
            CategoryId = 1,
            SupplierId = 1
        });

        productResponse.EnsureSuccessStatusCode();
        var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);

        var allResponse = await client.GetAsync("/api/inventory-items?q=A-01");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/inventory-items/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/inventory-items", new InventoryItemRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/inventory-items", new InventoryItemRequestDto
        {
            QuantityInStock = 10,
            MinimumQuantity = 2,
            MaximumQuantity = 50,
            ShelfLocation = "T-01-01",
            LastCheckedAt = DateTime.UtcNow,
            ProductId = product!.Id,
            WarehouseId = 1
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<InventoryItemDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/inventory-items/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/inventory-items/{created.Id}", new InventoryItemRequestDto
        {
            QuantityInStock = 20,
            MinimumQuantity = 2,
            MaximumQuantity = 60,
            ShelfLocation = "T-01-02",
            LastCheckedAt = DateTime.UtcNow,
            ProductId = product.Id,
            WarehouseId = 1
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/inventory-items/99999", new InventoryItemRequestDto
        {
            QuantityInStock = 20,
            MinimumQuantity = 2,
            MaximumQuantity = 60,
            ShelfLocation = "M-01-01",
            LastCheckedAt = DateTime.UtcNow,
            ProductId = product.Id,
            WarehouseId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/inventory-items/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/inventory-items/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task User_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/users?q=toni");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/users/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/users", new UserRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/users", new UserRequestDto
        {
            FirstName = "Integration",
            LastName = "User",
            Email = "integration.user@example.com",
            Role = UserRole.Customer,
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/users/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/users/{created.Id}", new UserRequestDto
        {
            FirstName = "Integration",
            LastName = "User Updated",
            Email = "integration.user.updated@example.com",
            Role = UserRole.Employee,
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/users/99999", new UserRequestDto
        {
            FirstName = "Missing",
            LastName = "User",
            Email = "missing.user@example.com",
            Role = UserRole.Customer,
            RegistrationDate = DateTime.UtcNow.Date,
            IsActive = true
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/users/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/users/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Order_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var allResponse = await client.GetAsync("/api/orders?q=ORD");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/orders/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/orders", new OrderRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/orders", new OrderRequestDto
        {
            OrderNumber = $"INT-{Guid.NewGuid():N}",
            OrderDate = DateTime.UtcNow,
            TotalPrice = 0m,
            Status = OrderStatus.Pending,
            DeliveryDate = null,
            Note = "Integration test order",
            UserId = 1
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/orders/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/orders/{created.Id}", new OrderRequestDto
        {
            OrderNumber = created.OrderNumber,
            OrderDate = DateTime.UtcNow,
            TotalPrice = 15m,
            Status = OrderStatus.Processing,
            DeliveryDate = null,
            Note = "Integration test order updated",
            UserId = 1
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/orders/99999", new OrderRequestDto
        {
            OrderNumber = $"MISSING-{Guid.NewGuid():N}",
            OrderDate = DateTime.UtcNow,
            TotalPrice = 15m,
            Status = OrderStatus.Processing,
            DeliveryDate = null,
            Note = "Missing order update",
            UserId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/orders/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/orders/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }

    [Fact]
    public async Task Order_item_api_supports_full_crud_and_validation()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var orderResponse = await client.PostAsJsonAsync("/api/orders", new OrderRequestDto
        {
            OrderNumber = $"ITEM-{Guid.NewGuid():N}",
            OrderDate = DateTime.UtcNow,
            TotalPrice = 0m,
            Status = OrderStatus.Pending,
            DeliveryDate = null,
            Note = "Order for order item test",
            UserId = 1
        });

        orderResponse.EnsureSuccessStatusCode();
        var order = await orderResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(order);

        var allResponse = await client.GetAsync("/api/order-items?q=Business");
        allResponse.EnsureSuccessStatusCode();

        var missingResponse = await client.GetAsync("/api/order-items/99999");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);

        var invalidResponse = await client.PostAsJsonAsync("/api/order-items", new OrderItemRequestDto());
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);

        var createResponse = await client.PostAsJsonAsync("/api/order-items", new OrderItemRequestDto
        {
            Quantity = 2,
            UnitPrice = 20m,
            TotalPrice = 40m,
            Discount = 0m,
            CreatedAt = DateTime.UtcNow,
            OrderId = order!.Id,
            ProductId = 1
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<OrderItemDto>();
        Assert.NotNull(created);

        var getByIdResponse = await client.GetAsync($"/api/order-items/{created!.Id}");
        getByIdResponse.EnsureSuccessStatusCode();

        var updateResponse = await client.PutAsJsonAsync($"/api/order-items/{created.Id}", new OrderItemRequestDto
        {
            Quantity = 3,
            UnitPrice = 20m,
            TotalPrice = 60m,
            Discount = 0m,
            CreatedAt = DateTime.UtcNow,
            OrderId = order.Id,
            ProductId = 1
        });

        updateResponse.EnsureSuccessStatusCode();

        var updateMissingResponse = await client.PutAsJsonAsync("/api/order-items/99999", new OrderItemRequestDto
        {
            Quantity = 3,
            UnitPrice = 20m,
            TotalPrice = 60m,
            Discount = 0m,
            CreatedAt = DateTime.UtcNow,
            OrderId = order.Id,
            ProductId = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, updateMissingResponse.StatusCode);

        var deleteResponse = await client.DeleteAsync($"/api/order-items/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteMissingResponse = await client.DeleteAsync("/api/order-items/99999");
        Assert.Equal(HttpStatusCode.NotFound, deleteMissingResponse.StatusCode);
    }
}
