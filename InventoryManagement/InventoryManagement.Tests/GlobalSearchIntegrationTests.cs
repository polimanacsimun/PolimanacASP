namespace InventoryManagement.Tests;

public class GlobalSearchIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GlobalSearchIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Global_search_returns_grouped_results_across_modules()
    {
        await _factory.ResetDatabaseAsync();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/search?q=Business");

        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Global Search", html);
        Assert.Contains("Business Laptop", html);
        Assert.Contains("Products", html);
        Assert.Contains("/catalog/1", html);
    }
}
