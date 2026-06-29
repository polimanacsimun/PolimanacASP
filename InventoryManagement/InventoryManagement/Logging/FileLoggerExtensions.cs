using Microsoft.Extensions.Logging;

namespace InventoryManagement.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder,
            string contentRootPath,
            Action<FileLoggerOptions>? configure = null)
        {
            var options = new FileLoggerOptions();
            configure?.Invoke(options);

            builder.AddProvider(new FileLoggerProvider(options, contentRootPath));

            return builder;
        }
    }
}
