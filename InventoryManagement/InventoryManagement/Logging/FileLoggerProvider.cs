using Microsoft.Extensions.Logging;

namespace InventoryManagement.Logging
{
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly FileLoggerOptions _options;
        private readonly object _syncRoot = new();

        public FileLoggerProvider(FileLoggerOptions options, string contentRootPath)
        {
            _options = options;
            _options.Path = ResolveLogPath(_options.Path, contentRootPath);

            var directory = Path.GetDirectoryName(_options.Path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _options, _syncRoot);
        }

        public void Dispose()
        {
        }

        private static string ResolveLogPath(string configuredPath, string contentRootPath)
        {
            if (Path.IsPathRooted(configuredPath))
            {
                return configuredPath;
            }

            return Path.Combine(contentRootPath, configuredPath);
        }
    }
}
