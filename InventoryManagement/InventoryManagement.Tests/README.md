# InventoryManagement.Tests

Integration tests for Lab 5 API endpoints.

- `ApiCrudIntegrationTests.cs` covers CRUD, validation, and missing-ID behavior for all API controllers under `/api`.
- `PlaywrightScenario.md` documents a 10-step browser scenario for admin login, navigation, and AJAX search flow.
- `PlaywrightUiSmokeTests.cs` automates the same 10-step scenario when Playwright environment variables are set.
- `FileLoggerTests.cs` verifies that the custom logging provider writes formatted entries to a log file.
- Product details include an AI advice action backed by OpenAI configuration; tests verify the safe fallback when no API key is configured.
- `../MCP_AGENTIC_IDE.md` documents the MCP / Agentic IDE workflow and the repository artifacts used as evidence.

Run the API integration tests from the solution folder:

```powershell
dotnet test InventoryManagement.slnx
```

Run the Playwright smoke test:

```powershell
cd .\InventoryManagement\InventoryManagement
dotnet run --urls http://localhost:5000
```

Then, in another PowerShell window:

```powershell
cd .\InventoryManagement
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5000"
$env:PLAYWRIGHT_ADMIN_EMAIL = "<admin-email>"
$env:PLAYWRIGHT_ADMIN_PASSWORD = "<admin-password>"
dotnet test .\InventoryManagement.Tests\InventoryManagement.Tests.csproj --filter PlaywrightUiSmokeTests
```

Useful Playwright switches:

```powershell
$env:HEADED = "1"      # show the browser
$env:BROWSER = "chromium"
$env:PWDEBUG = "1"    # interactive debug mode
```
