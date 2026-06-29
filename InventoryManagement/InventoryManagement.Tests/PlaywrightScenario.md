# Playwright Scenario - 10 Steps

Purpose: browser smoke scenario for the admin inventory UI. The API CRUD behavior is covered by integration tests; this scenario checks login, main navigation, and AJAX searches through the browser.

Prerequisites:

- Run the web app with seeded data and the seeded admin account.
- Use a Playwright project with `baseURL` pointing to the app, for example `http://localhost:5000`.
- Provide admin credentials through `PLAYWRIGHT_ADMIN_EMAIL` and `PLAYWRIGHT_ADMIN_PASSWORD`.

| Step | Action | Expected result |
| --- | --- | --- |
| 1 | Open `/Account/Login`. | Login page loads and shows `Welcome back`. |
| 2 | Enter admin email, enter admin password, and submit the form. | User is signed in and the navigation shows the `Admin` badge. |
| 3 | Open Products from the main navigation. | Product page loads with the `Products` heading and product table. |
| 4 | Search products for `Business`. | Product table shows `Business Laptop`. |
| 5 | Clear the product search. | Product search input is empty and the product table is repopulated. |
| 6 | Open Suppliers and search for `Alpha`. | Supplier table shows `Alpha Supply`. |
| 7 | Open Categories and search for `Electronics`. | Category table shows `Electronics`. |
| 8 | Open Warehouses and search for `Main`. | Warehouse table shows `Main Warehouse`. |
| 9 | Open Inventory and search for `A-01-01`. | Inventory table shows the matching shelf location. |
| 10 | Open Order Items and search for `ORD-2026-001`. | Order item table shows rows for that order. |
| 11 | Open Products and search for `Business`. | Product table shows `Business Laptop`. |
| 12 | Open edit form for `Business Laptop`. | Edit Product form is displayed. |
| 13 | Change the `Price` field to `1234.56` and save. | Product details page opens for `Business Laptop`. |
| 14 | Open Edit again for the same product. | Price input contains `1234.56`. |

Example Playwright implementation:

```ts
import { test, expect } from '@playwright/test';

test('inventory admin login, browsing, and search scenario - 10 steps', async ({ page }) => {
  await test.step('1. Open login page', async () => {
    await page.goto('/Account/Login');
    await expect(page.getByRole('heading', { name: 'Welcome back' })).toBeVisible();
  });

  await test.step('2. Log in as admin', async () => {
    await page.getByLabel('Email').fill(process.env.PLAYWRIGHT_ADMIN_EMAIL!);
    await page.getByLabel('Password').fill(process.env.PLAYWRIGHT_ADMIN_PASSWORD!);
    await page.getByRole('button', { name: 'Login' }).click();
    await expect(page.getByText('Admin').first()).toBeVisible();
  });

  await test.step('3. Open Products page', async () => {
    await page.getByRole('link', { name: 'Products' }).click();
    await expect(page.getByRole('heading', { name: 'Products' })).toBeVisible();
    await expect(page.locator('#productTableBody')).toBeVisible();
  });

  await test.step('4. Search products', async () => {
    await page.locator('#productSearchInput').fill('Business');
    await expect(page.locator('#productTableBody')).toContainText('Business Laptop');
  });

  await test.step('5. Clear product search', async () => {
    await page.locator('#clearProductSearch').click();
    await expect(page.locator('#productSearchInput')).toHaveValue('');
    await expect(page.locator('#productTableBody')).toContainText('Printer Paper A4');
  });

  await test.step('6. Search suppliers', async () => {
    await page.getByRole('link', { name: 'Suppliers' }).click();
    await expect(page.getByRole('heading', { name: 'Supplier Directory' })).toBeVisible();
    await page.locator('#supplierSearchInput').fill('Alpha');
    await expect(page.locator('#supplierTableBody')).toContainText('Alpha Supply');
  });

  await test.step('7. Search categories', async () => {
    await page.getByRole('link', { name: 'Categories' }).click();
    await page.locator('#categorySearchInput').fill('Electronics');
    await expect(page.locator('#categoryTableBody')).toContainText('Electronics');
  });

  await test.step('8. Search warehouses', async () => {
    await page.getByRole('link', { name: 'Warehouses' }).click();
    await page.locator('#warehouseSearchInput').fill('Main');
    await expect(page.locator('#warehouseTableBody')).toContainText('Main Warehouse');
  });

  await test.step('9. Search inventory records', async () => {
    await page.getByRole('link', { name: 'Inventory' }).click();
    await page.locator('#inventorySearchInput').fill('A-01-01');
    await expect(page.locator('#inventoryTableBody')).toContainText('A-01-01');
  });

  await test.step('10. Search order items', async () => {
    await page.getByRole('link', { name: 'Order Items' }).click();
    await page.locator('#orderItemSearchInput').fill('ORD-2026-001');
    await expect(page.locator('#orderItemTableBody')).toContainText('ORD-2026-001');
  });

  await test.step('11. Search product to edit', async () => {
    await page.getByRole('link', { name: 'Products' }).click();
    await page.locator('#productSearchInput').fill('Business');
    await expect(page.locator('#productTableBody')).toContainText('Business Laptop');
  });

  await test.step('12. Open product edit form', async () => {
    const row = page.locator('#productTableBody tr').filter({ hasText: 'Business Laptop' });
    await row.locator('a[title="Edit Product"]').click();
    await expect(page.locator('h4', { hasText: 'Edit Product' })).toBeVisible();
  });

  await test.step('13. Change product price and save', async () => {
    await page.getByLabel('Price').fill('1234.56');
    await page.getByRole('button', { name: 'Save Changes' }).click();
    await expect(page.getByRole('heading', { name: 'Business Laptop' })).toBeVisible();
  });

  await test.step('14. Verify saved product price', async () => {
    await page.locator('a[href="/catalog/1/edit"]').first().click();
    await expect(page.getByLabel('Price')).toHaveValue('1234.56');
  });
});
```
