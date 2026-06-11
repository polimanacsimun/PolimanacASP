using InventoryManagement.Domain.Models;

namespace InventoryManagement.DTOs;

public static class MappingExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name ?? string.Empty,
            Description = category.Description
        };
    }

    public static SupplierDto ToDto(this Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name ?? string.Empty,
            Address = supplier.Address,
            Phone = supplier.Phone,
            Email = supplier.Email,
            ContactPerson = supplier.ContactPerson,
            RegistrationDate = supplier.RegistrationDate,
            IsActive = supplier.IsActive
        };
    }

    public static WarehouseDto ToDto(this Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name ?? string.Empty,
            Address = warehouse.Address,
            Capacity = warehouse.Capacity,
            Phone = warehouse.Phone,
            Email = warehouse.Email,
            Manager = warehouse.Manager,
            OpeningDate = warehouse.OpeningDate,
            IsActive = warehouse.IsActive,
            Type = warehouse.Type
        };
    }

    public static ProductLookupDto ToLookupDto(this Product product)
    {
        return new ProductLookupDto
        {
            Id = product.Id,
            Name = product.Name ?? string.Empty,
            Price = product.Price,
            UnitOfMeasure = product.UnitOfMeasure ?? string.Empty
        };
    }

    public static ProductAttachmentDto ToDto(this ProductAttachment attachment)
    {
        return new ProductAttachmentDto
        {
            Id = attachment.Id,
            ProductId = attachment.ProductId,
            FileName = attachment.FileName ?? string.Empty,
            FilePath = attachment.FilePath ?? string.Empty,
            ContentType = attachment.ContentType ?? string.Empty,
            FileSize = attachment.FileSize,
            CreatedAt = attachment.CreatedAt
        };
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name ?? string.Empty,
            Description = product.Description,
            Price = product.Price,
            UnitOfMeasure = product.UnitOfMeasure ?? string.Empty,
            MinimumStock = product.MinimumStock,
            CreatedAt = product.CreatedAt,
            IsActive = product.IsActive,
            Type = product.Type,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            Category = product.Category?.ToDto(),
            Supplier = product.Supplier?.ToDto(),
            Attachments = product.Attachments?
                .Select(attachment => attachment.ToDto())
                .ToList() ?? new List<ProductAttachmentDto>()
        };
    }

    public static InventoryItemDto ToDto(this InventoryItem item)
    {
        return new InventoryItemDto
        {
            Id = item.Id,
            QuantityInStock = item.QuantityInStock,
            MinimumQuantity = item.MinimumQuantity,
            MaximumQuantity = item.MaximumQuantity,
            ShelfLocation = item.ShelfLocation,
            LastCheckedAt = item.LastCheckedAt,
            ProductId = item.ProductId,
            WarehouseId = item.WarehouseId,
            Product = item.Product?.ToLookupDto(),
            Warehouse = item.Warehouse?.ToDto()
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.Role,
            RegistrationDate = user.RegistrationDate,
            IsActive = user.IsActive
        };
    }

    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto
        {
            Id = item.Id,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.TotalPrice,
            Discount = item.Discount,
            CreatedAt = item.CreatedAt,
            OrderId = item.OrderId,
            ProductId = item.ProductId,
            Product = item.Product?.ToLookupDto()
        };
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber ?? string.Empty,
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            DeliveryDate = order.DeliveryDate,
            Note = order.Note,
            UserId = order.UserId,
            User = order.User?.ToDto(),
            OrderItems = order.OrderItems?
                .Select(item => item.ToDto())
                .ToList() ?? new List<OrderItemDto>()
        };
    }
}
