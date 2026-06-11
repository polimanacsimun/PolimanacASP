using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class SupplierDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }
}

public class WarehouseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int Capacity { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Manager { get; set; }
    public DateTime OpeningDate { get; set; }
    public bool IsActive { get; set; }
    public WarehouseType Type { get; set; }
}

public class ProductLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
}

public class ProductAttachmentDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public int MinimumStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public ProductType Type { get; set; }

    public int? CategoryId { get; set; }
    public CategoryDto? Category { get; set; }

    public int? SupplierId { get; set; }
    public SupplierDto? Supplier { get; set; }

    public List<ProductAttachmentDto> Attachments { get; set; } = new();
}

public class InventoryItemDto
{
    public int Id { get; set; }
    public int QuantityInStock { get; set; }
    public int MinimumQuantity { get; set; }
    public int MaximumQuantity { get; set; }
    public string? ShelfLocation { get; set; }
    public DateTime LastCheckedAt { get; set; }

    public int ProductId { get; set; }
    public ProductLookupDto? Product { get; set; }

    public int WarehouseId { get; set; }
    public WarehouseDto? Warehouse { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }
    public DateTime CreatedAt { get; set; }

    public int OrderId { get; set; }
    public int ProductId { get; set; }

    public ProductLookupDto? Product { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Note { get; set; }

    public int UserId { get; set; }
    public UserDto? User { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class CategoryRequestDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}

public class SupplierRequestDto
{
    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? ContactPerson { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public bool? IsActive { get; set; }
}

public class WarehouseRequestDto
{
    [Required(ErrorMessage = "Warehouse name is required.")]
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [Range(0, 1000000)]
    public int? Capacity { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Manager { get; set; }

    public DateTime? OpeningDate { get; set; }

    public bool? IsActive { get; set; }

    [Required(ErrorMessage = "Warehouse type is required.")]
    public WarehouseType? Type { get; set; }
}

public class ProductRequestDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 100000)]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Unit of measure is required.")]
    [StringLength(30)]
    public string? UnitOfMeasure { get; set; }

    [Range(0, 100000)]
    public int? MinimumStock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    [Required(ErrorMessage = "Product type is required.")]
    public ProductType? Type { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Supplier is required.")]
    public int? SupplierId { get; set; }
}

public class InventoryItemRequestDto
{
    [Required(ErrorMessage = "Quantity in stock is required.")]
    [Range(0, 1000000)]
    public int? QuantityInStock { get; set; }

    [Required(ErrorMessage = "Minimum quantity is required.")]
    [Range(0, 1000000)]
    public int? MinimumQuantity { get; set; }

    [Required(ErrorMessage = "Maximum quantity is required.")]
    [Range(0, 1000000)]
    public int? MaximumQuantity { get; set; }

    [StringLength(50)]
    public string? ShelfLocation { get; set; }

    public DateTime? LastCheckedAt { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    public int? ProductId { get; set; }

    [Required(ErrorMessage = "Warehouse is required.")]
    public int? WarehouseId { get; set; }
}

public class UserRequestDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public UserRole? Role { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public bool? IsActive { get; set; }
}

public class OrderRequestDto
{
    [Required(ErrorMessage = "Order number is required.")]
    [StringLength(50)]
    public string? OrderNumber { get; set; }

    public DateTime? OrderDate { get; set; }

    [Required(ErrorMessage = "Total price is required.")]
    [Range(0, 10000000)]
    public decimal? TotalPrice { get; set; }

    [Required(ErrorMessage = "Order status is required.")]
    public OrderStatus? Status { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    [Required(ErrorMessage = "User is required.")]
    public int? UserId { get; set; }
}

public class OrderItemRequestDto
{
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 1000000)]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Unit price is required.")]
    [Range(0.01, 10000000)]
    public decimal? UnitPrice { get; set; }

    [Range(0, 10000000)]
    public decimal? TotalPrice { get; set; }

    [Range(0, 100)]
    public decimal? Discount { get; set; }

    public DateTime? CreatedAt { get; set; }

    [Required(ErrorMessage = "Order is required.")]
    public int? OrderId { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    public int? ProductId { get; set; }
}
