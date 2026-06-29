namespace InventoryManagement.Tests;

public class ProductAiAdviceIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ProductAiAdviceIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Product_ai_advice_shows_configuration_message_without_api_key()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var detailsResponse = await client.GetAsync("/catalog/1");
        detailsResponse.EnsureSuccessStatusCode();
        var detailsHtml = await detailsResponse.Content.ReadAsStringAsync();
        var token = AntiForgeryTokenReader.Read(detailsHtml);

        var response = await client.PostAsync(
            "/catalog/1/ai-advice",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = token
            }));

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("AI product advice", html);
        Assert.Contains("AI setup required", html);
        Assert.Contains("AI assistant is not configured", html);
    }
}
