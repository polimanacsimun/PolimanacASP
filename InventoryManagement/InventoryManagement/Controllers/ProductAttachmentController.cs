using InventoryManagement.DAL;
using InventoryManagement.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("products/{productId:int}/attachments")]
    public class ProductAttachmentController : Controller
    {
        private readonly InventoryManagementDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public ProductAttachmentController(
            InventoryManagementDbContext dbContext,
            IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> List(int productId)
        {
            var attachments = await GetProductAttachments(productId);
            return PartialView("~/Views/Product/_ProductAttachments.cshtml", attachments);
        }

        [HttpPost("upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int productId, List<IFormFile> files)
        {
            var productExists = await _dbContext.Products.AnyAsync(p => p.Id == productId);

            if (!productExists)
            {
                return NotFound();
            }

            if (files == null || files.Count == 0)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new { message = "Please choose at least one file." });
            }

            var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "application/pdf",
                "image/jpeg",
                "image/png",
                "image/gif",
                "text/plain",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            const long maxFileSize = 10 * 1024 * 1024;

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "products",
                productId.ToString());

            Directory.CreateDirectory(uploadFolder);

            foreach (var file in files.Where(f => f.Length > 0))
            {
                if (file.Length > maxFileSize)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Json(new { message = $"File {file.FileName} is larger than 10 MB." });
                }

                if (!allowedContentTypes.Contains(file.ContentType))
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Json(new { message = $"File type is not allowed: {file.FileName}" });
                }

                var extension = Path.GetExtension(file.FileName);
                var storedFileName = $"{Guid.NewGuid():N}{extension}";
                var physicalPath = Path.Combine(uploadFolder, storedFileName);

                await using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var attachment = new ProductAttachment
                {
                    ProductId = productId,
                    FileName = Path.GetFileName(file.FileName),
                    FilePath = $"/uploads/products/{productId}/{storedFileName}",
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.ProductAttachments.Add(attachment);
            }

            await _dbContext.SaveChangesAsync();

            var attachments = await GetProductAttachments(productId);
            return PartialView("~/Views/Product/_ProductAttachments.cshtml", attachments);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{attachmentId:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int productId, int attachmentId)
        {
            var attachment = await _dbContext.ProductAttachments
                .FirstOrDefaultAsync(a => a.Id == attachmentId && a.ProductId == productId);

            if (attachment == null)
            {
                return NotFound();
            }

            var relativePath = attachment.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            _dbContext.ProductAttachments.Remove(attachment);
            await _dbContext.SaveChangesAsync();

            var attachments = await GetProductAttachments(productId);
            return PartialView("~/Views/Product/_ProductAttachments.cshtml", attachments);
        }

        private async Task<List<ProductAttachment>> GetProductAttachments(int productId)
        {
            return await _dbContext.ProductAttachments
                .AsNoTracking()
                .Where(a => a.ProductId == productId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
