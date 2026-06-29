using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace InventoryManagement.Tests;

public class PlaywrightUiSmokeTests
{
    [Fact]
    public async Task Admin_can_login_and_complete_10_step_browsing_and_search_scenario()
    {
        var baseUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL");
        var adminEmail = Environment.GetEnvironmentVariable("PLAYWRIGHT_ADMIN_EMAIL");
        var adminPassword = Environment.GetEnvironmentVariable("PLAYWRIGHT_ADMIN_PASSWORD");

        if (string.IsNullOrWhiteSpace(baseUrl) ||
            string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("HEADED"))
        });

        var page = await browser.NewPageAsync(new()
        {
            ViewportSize = new()
            {
                Width = 1440,
                Height = 950
            }
        });

        page.SetDefaultTimeout(10_000);

        await page.GotoAsync(new Uri(new Uri(baseUrl), "/Account/Login").ToString());
        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome back" })).ToBeVisibleAsync();

        await page.GetByLabel("Email").FillAsync(adminEmail);
        await page.GetByLabel("Password").FillAsync(adminPassword);
        await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Expect(page.GetByText("Admin").First).ToBeVisibleAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Products", Exact = true }).First.ClickAsync();
        await Expect(page.Locator("h1", new() { HasTextString = "Products" })).ToBeVisibleAsync();
        await Expect(page.Locator("#productTableBody")).ToBeVisibleAsync();

        await page.Locator("#productSearchInput").FillAsync("Business");
        await Expect(page.Locator("#productTableBody")).ToContainTextAsync("Business Laptop");

        await page.Locator("#clearProductSearch").ClickAsync();
        await Expect(page.Locator("#productSearchInput")).ToHaveValueAsync(string.Empty);
        await Expect(page.Locator("#productTableBody")).ToContainTextAsync("Printer Paper A4");

        await page.GetByRole(AriaRole.Link, new() { Name = "Suppliers", Exact = true }).First.ClickAsync();
        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Supplier Directory" })).ToBeVisibleAsync();

        await page.Locator("#supplierSearchInput").FillAsync("Alpha");
        await Expect(page.Locator("#supplierTableBody")).ToContainTextAsync("Alpha Supply");

        await page.GetByRole(AriaRole.Link, new() { Name = "Categories", Exact = true }).First.ClickAsync();
        await page.Locator("#categorySearchInput").FillAsync("Electronics");
        await Expect(page.Locator("#categoryTableBody")).ToContainTextAsync("Electronics");

        await page.GetByRole(AriaRole.Link, new() { Name = "Warehouses", Exact = true }).First.ClickAsync();
        await page.Locator("#warehouseSearchInput").FillAsync("Main");
        await Expect(page.Locator("#warehouseTableBody")).ToContainTextAsync("Main Warehouse");

        await page.GetByRole(AriaRole.Link, new() { Name = "Inventory", Exact = true }).First.ClickAsync();
        await page.Locator("#inventorySearchInput").FillAsync("A-01-01");
        await Expect(page.Locator("#inventoryTableBody")).ToContainTextAsync("A-01-01");

        await page.GetByRole(AriaRole.Link, new() { Name = "Order Items", Exact = true }).First.ClickAsync();
        await page.Locator("#orderItemSearchInput").FillAsync("ORD-2026-001");
        await Expect(page.Locator("#orderItemTableBody")).ToContainTextAsync("ORD-2026-001");

        await page.GetByRole(AriaRole.Link, new() { Name = "Products", Exact = true }).First.ClickAsync();
        await page.Locator("#productSearchInput").FillAsync("Business");

        var businessLaptopRow = page.Locator("#productTableBody tr").Filter(new() { HasTextString = "Business Laptop" });
        await businessLaptopRow.Locator("a[title='Edit Product']").ClickAsync();

        await Expect(page.Locator("h4", new() { HasTextString = "Edit Product" })).ToBeVisibleAsync();
        await page.GetByLabel("Price").FillAsync("1234.56");
        await page.GetByRole(AriaRole.Button, new() { Name = "Save Changes" }).ClickAsync();

        await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Business Laptop" })).ToBeVisibleAsync();
        await page.Locator("a[href='/catalog/1/edit']").First.ClickAsync();
        await Expect(page.GetByLabel("Price")).ToHaveValueAsync("1234.56");
    }
}
