using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Models
{
    public class ProductAttachment
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
