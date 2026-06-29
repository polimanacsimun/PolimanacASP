using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace InventoryManagement.Tests;

public class PlaywrightUiSmokeTests : PageTest
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

        Page.SetDefaultTimeout(10_000);
        await Page.SetViewportSizeAsync(1440, 950);

        await Page.GotoAsync(new Uri(new Uri(baseUrl), "/Account/Login").ToString());
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome back" })).ToBeVisibleAsync();

        await Page.GetByLabel("Email").FillAsync(adminEmail);
        await Page.GetByLabel("Password").FillAsync(adminPassword);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Expect(Page.GetByText("Admin").First).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Products", Exact = true }).First.ClickAsync();
        await Expect(Page.Locator("h1", new() { HasTextString = "Products" })).ToBeVisibleAsync();
        await Expect(Page.Locator("#productTableBody")).ToBeVisibleAsync();

        await Page.Locator("#productSearchInput").FillAsync("Business");
        await Expect(Page.Locator("#productTableBody")).ToContainTextAsync("Business Laptop");

        await Page.Locator("#clearProductSearch").ClickAsync();
        await Expect(Page.Locator("#productSearchInput")).ToHaveValueAsync(string.Empty);
        await Expect(Page.Locator("#productTableBody")).ToContainTextAsync("Printer Paper A4");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Suppliers", Exact = true }).First.ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Supplier Directory" })).ToBeVisibleAsync();

        await Page.Locator("#supplierSearchInput").FillAsync("Alpha");
        await Expect(Page.Locator("#supplierTableBody")).ToContainTextAsync("Alpha Supply");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Categories", Exact = true }).First.ClickAsync();
        await Page.Locator("#categorySearchInput").FillAsync("Electronics");
        await Expect(Page.Locator("#categoryTableBody")).ToContainTextAsync("Electronics");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Warehouses", Exact = true }).First.ClickAsync();
        await Page.Locator("#warehouseSearchInput").FillAsync("Main");
        await Expect(Page.Locator("#warehouseTableBody")).ToContainTextAsync("Main Warehouse");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Inventory", Exact = true }).First.ClickAsync();
        await Page.Locator("#inventorySearchInput").FillAsync("A-01-01");
        await Expect(Page.Locator("#inventoryTableBody")).ToContainTextAsync("A-01-01");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Order Items", Exact = true }).First.ClickAsync();
        await Page.Locator("#orderItemSearchInput").FillAsync("ORD-2026-001");
        await Expect(Page.Locator("#orderItemTableBody")).ToContainTextAsync("ORD-2026-001");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Products", Exact = true }).First.ClickAsync();
        await Page.Locator("#productSearchInput").FillAsync("Business");

        var businessLaptopRow = Page.Locator("#productTableBody tr").Filter(new() { HasTextString = "Business Laptop" });
        await businessLaptopRow.Locator("a[title='Edit Product']").ClickAsync();

        await Expect(Page.Locator("h4", new() { HasTextString = "Edit Product" })).ToBeVisibleAsync();
        await Page.GetByLabel("Price").FillAsync("1234.56");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Save Changes" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Business Laptop" })).ToBeVisibleAsync();
        await Page.Locator("a[href='/catalog/1/edit']").First.ClickAsync();
        await Expect(Page.GetByLabel("Price")).ToHaveValueAsync("1234.56");
    }
}
