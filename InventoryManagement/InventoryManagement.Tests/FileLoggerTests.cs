using InventoryManagement.Logging;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Tests
{
    public class FileLoggerTests
    {
        [Fact]
        public void FileLogger_WritesFormattedLogEntry()
        {
            var logFileName = $"{Guid.NewGuid()}.log";
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "TestLogs");
            var relativeLogPath = Path.Combine("TestLogs", logFileName);

            using var provider = new FileLoggerProvider(
                new FileLoggerOptions
                {
                    Path = relativeLogPath,
                    MinimumLevel = LogLevel.Information,
                    MaxFileSizeBytes = 1024 * 1024
                },
                AppContext.BaseDirectory);

            var logger = provider.CreateLogger("InventoryManagement.Tests.FileLoggerTests");

            logger.LogInformation("Created test product {ProductId}.", 42);

            var logPath = Path.Combine(logDirectory, logFileName);
            var logContent = File.ReadAllText(logPath);

            Assert.Contains("Information", logContent);
            Assert.Contains("InventoryManagement.Tests.FileLoggerTests", logContent);
            Assert.Contains("Created test product 42.", logContent);
        }
    }
}
