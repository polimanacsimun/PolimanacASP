using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Logging
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly FileLoggerOptions _options;
        private readonly object _syncRoot;

        public FileLogger(string categoryName, FileLoggerOptions options, object syncRoot)
        {
            _categoryName = categoryName;
            _options = options;
            _syncRoot = syncRoot;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && logLevel >= _options.MinimumLevel;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message) && exception == null)
            {
                return;
            }

            var entry = FormatEntry(logLevel, eventId, message, exception);

            lock (_syncRoot)
            {
                RotateIfNeeded(entry.Length);
                File.AppendAllText(_options.Path, entry, Encoding.UTF8);
            }
        }

        private string FormatEntry(LogLevel logLevel, EventId eventId, string message, Exception? exception)
        {
            var builder = new StringBuilder();
            builder
                .Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture))
                .Append(" | ")
                .Append(logLevel)
                .Append(" | ")
                .Append(_categoryName);

            if (eventId.Id != 0 || !string.IsNullOrWhiteSpace(eventId.Name))
            {
                builder
                    .Append(" | EventId: ")
                    .Append(eventId.Id);

                if (!string.IsNullOrWhiteSpace(eventId.Name))
                {
                    builder
                        .Append(" (")
                        .Append(eventId.Name)
                        .Append(')');
                }
            }

            builder
                .Append(" | ")
                .AppendLine(message);

            if (exception != null)
            {
                builder.AppendLine(exception.ToString());
            }

            return builder.ToString();
        }

        private void RotateIfNeeded(int nextEntryLength)
        {
            var fileInfo = new FileInfo(_options.Path);
            if (!fileInfo.Exists || _options.MaxFileSizeBytes <= 0)
            {
                return;
            }

            if (fileInfo.Length + nextEntryLength <= _options.MaxFileSizeBytes)
            {
                return;
            }

            var archivePath = Path.Combine(
                fileInfo.DirectoryName ?? string.Empty,
                $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}-{DateTime.Now:yyyyMMddHHmmss}{fileInfo.Extension}");

            File.Move(fileInfo.FullName, archivePath, overwrite: true);
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
