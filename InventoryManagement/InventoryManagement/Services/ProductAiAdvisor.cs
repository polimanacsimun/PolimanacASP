using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.Services;

public interface IProductAiAdvisor
{
    Task<ProductAiAdvice> GetAdviceAsync(Product product, CancellationToken cancellationToken = default);
}

public sealed record ProductAiAdvice(bool IsConfigured, string Text);

public class ProductAiAdvisor : IProductAiAdvisor
{
    private const string OpenAiResponsesEndpoint = "https://api.openai.com/v1/responses";

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProductAiAdvisor> _logger;

    public ProductAiAdvisor(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ProductAiAdvisor> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ProductAiAdvice> GetAdviceAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return new ProductAiAdvice(
                false,
                "AI assistant is not configured. Add an OpenAI API key to enable live product advice.");
        }

        var model = _configuration["OpenAI:Model"] ?? "gpt-4.1-mini";
        var prompt = BuildPrompt(product);

        using var request = new HttpRequestMessage(HttpMethod.Post, OpenAiResponsesEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new
            {
                model,
                input = prompt,
                max_output_tokens = 220
            }),
            Encoding.UTF8,
            "application/json");

        try
        {
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "OpenAI advice request failed with status {StatusCode}: {ResponseBody}",
                    response.StatusCode,
                    responseBody);

                return BuildFailureAdvice(response.StatusCode, responseBody);
            }

            var text = ExtractText(responseBody);
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ProductAiAdvice(
                    true,
                    "AI assistant returned an empty response.");
            }

            return new ProductAiAdvice(true, text.Trim());
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "OpenAI advice request failed.");
            return new ProductAiAdvice(
                true,
                "AI assistant could not generate advice right now. Please try again later.");
        }
    }

    private static string BuildPrompt(Product product)
    {
        var currentStock = product.InventoryItems.Sum(item => item.QuantityInStock);
        var category = product.Category?.Name ?? "Unassigned";
        var supplier = product.Supplier?.Name ?? "Unassigned";

        return $"""
            You are an inventory operations assistant. Give concise product advice in 3 bullet points.
            Focus on reorder risk, stock strategy, and one operational next step.

            Product:
            - Name: {product.Name}
            - Description: {product.Description}
            - Category: {category}
            - Supplier: {supplier}
            - Type: {product.Type}
            - Unit price: {product.Price}
            - Unit of measure: {product.UnitOfMeasure}
            - Minimum stock: {product.MinimumStock}
            - Current stock across warehouses: {currentStock}
            """;
    }

    private static string? ExtractText(string responseBody)
    {
        using var document = JsonDocument.Parse(responseBody);
        var root = document.RootElement;

        if (root.TryGetProperty("output_text", out var outputText) &&
            outputText.ValueKind == JsonValueKind.String)
        {
            return outputText.GetString();
        }

        if (!root.TryGetProperty("output", out var output) ||
            output.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        var builder = new StringBuilder();
        foreach (var outputItem in output.EnumerateArray())
        {
            if (!outputItem.TryGetProperty("content", out var content) ||
                content.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var contentItem in content.EnumerateArray())
            {
                if (contentItem.TryGetProperty("text", out var text) &&
                    text.ValueKind == JsonValueKind.String)
                {
                    builder.AppendLine(text.GetString());
                }
            }
        }

        return builder.ToString();
    }

    private static ProductAiAdvice BuildFailureAdvice(HttpStatusCode statusCode, string responseBody)
    {
        var errorCode = TryReadOpenAiErrorCode(responseBody);

        if (statusCode == HttpStatusCode.Unauthorized)
        {
            return new ProductAiAdvice(
                true,
                "OpenAI rejected the configured API key. Check whether the key is valid and active.");
        }

        if ((int)statusCode == 429 && errorCode == "insufficient_quota")
        {
            return new ProductAiAdvice(
                true,
                "OpenAI quota is exhausted for the configured API key. Check billing, quota, or use another active key.");
        }

        if ((int)statusCode == 429)
        {
            return new ProductAiAdvice(
                true,
                "OpenAI rate limit was reached. Wait briefly and try generating advice again.");
        }

        return new ProductAiAdvice(
            true,
            "AI assistant could not generate advice right now. Please try again later.");
    }

    private static string? TryReadOpenAiErrorCode(string responseBody)
    {
        try
        {
            using var document = JsonDocument.Parse(responseBody);

            if (document.RootElement.TryGetProperty("error", out var error) &&
                error.TryGetProperty("code", out var code) &&
                code.ValueKind == JsonValueKind.String)
            {
                return code.GetString();
            }
        }
        catch (JsonException)
        {
        }

        return null;
    }
}
