using Microsoft.Extensions.Logging;

namespace InventoryManagement.Logging
{
    public class FileLoggerOptions
    {
        public string Path { get; set; } = "Logs/inventory-management.log";

        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

        public long MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024;
    }
}
