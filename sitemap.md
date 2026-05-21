# Sitemap and Routing Model

## Project

This document describes the available URLs in the InventoryManagement ASP.NET Core MVC application.

The application uses the default MVC route as a fallback:

`/{controller}/{action}/{id?}`

In Lab 3, additional semantic custom routes were added using attribute routing.  
The default route is still available, but custom routes are used to make selected URLs more meaningful and business-oriented.

---

## Lab 4: CRUD Operations and AJAX Features

Lab 4 implemented complete CRUD (Create, Read, Update, Delete) operations for all entities with the following features:

- **Form Models**: ViewModels with DataAnnotation validation attributes
- **Server-side Validation**: ModelState.IsValid checks and error handling
- **Client-side Validation**: jQuery validation framework
- **AJAX Search**: Dynamic table updates with debounce (300ms) and fade animations
- **Autocomplete Dropdowns**: Reusable component for entity relationship selection
- **Custom Date Input**: Language-aware date/time input with ISO storage format
- **Delete Safety**: Dependency checks prevent deletion of records with related data
- **Toast Notifications**: TempData-based success messages after CRUD operations
- **UI Polish**: Delete confirmation dialogs, row highlighting, page load animations

All CRUD routes support both semantic URLs (business-friendly) and default MVC routes (fallback).

---

## Home Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/` | HomeController | Index | Views/Home/Index.cshtml | Main dashboard page. |
| `/dashboard` | HomeController | Index | Views/Home/Index.cshtml | Custom semantic dashboard route. |
| `/Home/Index` | HomeController | Index | Views/Home/Index.cshtml | Default MVC route. |
| `/Home/Privacy` | HomeController | Privacy | Views/Home/Privacy.cshtml | Standard privacy page. |
| `/Home/Error` | HomeController | Error | Views/Shared/Error.cshtml | Error page. |

---

## Product Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/Product` | ProductController | Index | Views/Product/Index.cshtml | Default MVC route for product list. |
| `/Product/Index` | ProductController | Index | Views/Product/Index.cshtml | Default MVC route for product list. |
| `/Product/Details/{id}` | ProductController | Details | Views/Product/Details.cshtml | Default MVC route for product details. |
| `/Product/Create` | ProductController | Create | Views/Product/Create.cshtml | Default MVC route for product creation form. |
| `/Product/Create` (POST) | ProductController | Create | Redirect | Submits new product data. |
| `/Product/Edit/{id}` | ProductController | Edit | Views/Product/Edit.cshtml | Default MVC route for product edit form. |
| `/Product/Edit/{id}` (POST) | ProductController | Edit | Redirect | Submits product updates. |
| `/Product/Delete/{id}` | ProductController | Delete | Views/Product/Delete.cshtml | Default MVC route for delete confirmation. |
| `/Product/Delete/{id}` (POST) | ProductController | DeleteConfirmed | Redirect | Confirms product deletion. |
| `/catalog` | ProductController | Index | Views/Product/Index.cshtml | Custom semantic route for product list. |
| `/catalog/{id:int}` | ProductController | Details | Views/Product/Details.cshtml | Custom semantic route for product details. |
| `/catalog/create` | ProductController | Create | Views/Product/Create.cshtml | Custom semantic route for product creation form. |
| `/catalog/create` (POST) | ProductController | Create | Redirect | Submits new product data. |
| `/catalog/{id:int}/edit` | ProductController | Edit | Views/Product/Edit.cshtml | Custom semantic route for product edit form. |
| `/catalog/{id:int}/edit` (POST) | ProductController | Edit | Redirect | Submits product updates. |
| `/catalog/{id:int}/delete` | ProductController | Delete | Views/Product/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/catalog/{id:int}/delete` (POST) | ProductController | DeleteConfirmed | Redirect | Confirms product deletion. |
| `/catalog/search` | ProductController | Search | Views/Product/_ProductTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## Supplier Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/Supplier` | SupplierController | Index | Views/Supplier/Index.cshtml | Default MVC route for supplier list. |
| `/Supplier/Index` | SupplierController | Index | Views/Supplier/Index.cshtml | Default MVC route for supplier list. |
| `/Supplier/Details/{id}` | SupplierController | Details | Views/Supplier/Details.cshtml | Default MVC route for supplier details. |
| `/Supplier/Create` | SupplierController | Create | Views/Supplier/Create.cshtml | Default MVC route for supplier creation form. |
| `/Supplier/Create` (POST) | SupplierController | Create | Redirect | Submits new supplier data. |
| `/Supplier/Edit/{id}` | SupplierController | Edit | Views/Supplier/Edit.cshtml | Default MVC route for supplier edit form. |
| `/Supplier/Edit/{id}` (POST) | SupplierController | Edit | Redirect | Submits supplier updates. |
| `/Supplier/Delete/{id}` | SupplierController | Delete | Views/Supplier/Delete.cshtml | Default MVC route for delete confirmation. |
| `/Supplier/Delete/{id}` (POST) | SupplierController | DeleteConfirmed | Redirect | Confirms supplier deletion. |
| `/vendors` | SupplierController | Index | Views/Supplier/Index.cshtml | Custom semantic route for supplier list. |
| `/vendors/{id:int}` | SupplierController | Details | Views/Supplier/Details.cshtml | Custom semantic route for supplier details. |
| `/vendors/create` | SupplierController | Create | Views/Supplier/Create.cshtml | Custom semantic route for supplier creation form. |
| `/vendors/create` (POST) | SupplierController | Create | Redirect | Submits new supplier data. |
| `/vendors/{id:int}/edit` | SupplierController | Edit | Views/Supplier/Edit.cshtml | Custom semantic route for supplier edit form. |
| `/vendors/{id:int}/edit` (POST) | SupplierController | Edit | Redirect | Submits supplier updates. |
| `/vendors/{id:int}/delete` | SupplierController | Delete | Views/Supplier/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/vendors/{id:int}/delete` (POST) | SupplierController | DeleteConfirmed | Redirect | Confirms supplier deletion. |
| `/vendors/search` | SupplierController | Search | Views/Supplier/_SupplierTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## Category Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/Category` | CategoryController | Index | Views/Category/Index.cshtml | Default MVC route for category list. |
| `/Category/Index` | CategoryController | Index | Views/Category/Index.cshtml | Default MVC route for category list. |
| `/Category/Details/{id}` | CategoryController | Details | Views/Category/Details.cshtml | Default MVC route for category details. |
| `/Category/Create` | CategoryController | Create | Views/Category/Create.cshtml | Default MVC route for category creation form. |
| `/Category/Create` (POST) | CategoryController | Create | Redirect | Submits new category data. |
| `/Category/Edit/{id}` | CategoryController | Edit | Views/Category/Edit.cshtml | Default MVC route for category edit form. |
| `/Category/Edit/{id}` (POST) | CategoryController | Edit | Redirect | Submits category updates. |
| `/Category/Delete/{id}` | CategoryController | Delete | Views/Category/Delete.cshtml | Default MVC route for delete confirmation. |
| `/Category/Delete/{id}` (POST) | CategoryController | DeleteConfirmed | Redirect | Confirms category deletion. |
| `/Category/Search` | CategoryController | Search | Views/Category/_CategoryTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## Warehouse Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/Warehouse` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Default MVC route for warehouse list. |
| `/Warehouse/Index` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Default MVC route for warehouse list. |
| `/Warehouse/Details/{id}` | WarehouseController | Details | Views/Warehouse/Details.cshtml | Default MVC route for warehouse details. |
| `/Warehouse/Create` | WarehouseController | Create | Views/Warehouse/Create.cshtml | Default MVC route for warehouse creation form. |
| `/Warehouse/Create` (POST) | WarehouseController | Create | Redirect | Submits new warehouse data. |
| `/Warehouse/Edit/{id}` | WarehouseController | Edit | Views/Warehouse/Edit.cshtml | Default MVC route for warehouse edit form. |
| `/Warehouse/Edit/{id}` (POST) | WarehouseController | Edit | Redirect | Submits warehouse updates. |
| `/Warehouse/Delete/{id}` | WarehouseController | Delete | Views/Warehouse/Delete.cshtml | Default MVC route for delete confirmation. |
| `/Warehouse/Delete/{id}` (POST) | WarehouseController | DeleteConfirmed | Redirect | Confirms warehouse deletion. |
| `/storage` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Custom semantic route for warehouse list. |
| `/storage/{id:int}` | WarehouseController | Details | Views/Warehouse/Details.cshtml | Custom semantic route for warehouse details. |
| `/storage/create` | WarehouseController | Create | Views/Warehouse/Create.cshtml | Custom semantic route for warehouse creation form. |
| `/storage/create` (POST) | WarehouseController | Create | Redirect | Submits new warehouse data. |
| `/storage/{id:int}/edit` | WarehouseController | Edit | Views/Warehouse/Edit.cshtml | Custom semantic route for warehouse edit form. |
| `/storage/{id:int}/edit` (POST) | WarehouseController | Edit | Redirect | Submits warehouse updates. |
| `/storage/{id:int}/delete` | WarehouseController | Delete | Views/Warehouse/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/storage/{id:int}/delete` (POST) | WarehouseController | DeleteConfirmed | Redirect | Confirms warehouse deletion. |
| `/storage/search` | WarehouseController | Search | Views/Warehouse/_WarehouseTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## User Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/User` | UserController | Index | Views/User/Index.cshtml | Default MVC route for user list. |
| `/User/Index` | UserController | Index | Views/User/Index.cshtml | Default MVC route for user list. |
| `/User/Details/{id}` | UserController | Details | Views/User/Details.cshtml | Default MVC route for user details. |
| `/User/Create` | UserController | Create | Views/User/Create.cshtml | Default MVC route for user creation form. |
| `/User/Create` (POST) | UserController | Create | Redirect | Submits new user data. |
| `/User/Edit/{id}` | UserController | Edit | Views/User/Edit.cshtml | Default MVC route for user edit form. |
| `/User/Edit/{id}` (POST) | UserController | Edit | Redirect | Submits user updates. |
| `/User/Delete/{id}` | UserController | Delete | Views/User/Delete.cshtml | Default MVC route for delete confirmation. |
| `/User/Delete/{id}` (POST) | UserController | DeleteConfirmed | Redirect | Confirms user deletion. |
| `/users` | UserController | Index | Views/User/Index.cshtml | Custom semantic route for user list. |
| `/users/{id:int}` | UserController | Details | Views/User/Details.cshtml | Custom semantic route for user details. |
| `/users/create` | UserController | Create | Views/User/Create.cshtml | Custom semantic route for user creation form. |
| `/users/create` (POST) | UserController | Create | Redirect | Submits new user data. |
| `/users/{id:int}/edit` | UserController | Edit | Views/User/Edit.cshtml | Custom semantic route for user edit form. |
| `/users/{id:int}/edit` (POST) | UserController | Edit | Redirect | Submits user updates. |
| `/users/{id:int}/delete` | UserController | Delete | Views/User/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/users/{id:int}/delete` (POST) | UserController | DeleteConfirmed | Redirect | Confirms user deletion. |
| `/users/search` | UserController | Search | Views/User/_UserTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## Order Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/Order` | OrderController | Index | Views/Order/Index.cshtml | Default MVC route for order list. |
| `/Order/Index` | OrderController | Index | Views/Order/Index.cshtml | Default MVC route for order list. |
| `/Order/Details/{id}` | OrderController | Details | Views/Order/Details.cshtml | Default MVC route for order details. |
| `/Order/Create` | OrderController | Create | Views/Order/Create.cshtml | Default MVC route for order creation form. |
| `/Order/Create` (POST) | OrderController | Create | Redirect | Submits new order data. |
| `/Order/Edit/{id}` | OrderController | Edit | Views/Order/Edit.cshtml | Default MVC route for order edit form. |
| `/Order/Edit/{id}` (POST) | OrderController | Edit | Redirect | Submits order updates. |
| `/Order/Delete/{id}` | OrderController | Delete | Views/Order/Delete.cshtml | Default MVC route for delete confirmation. |
| `/Order/Delete/{id}` (POST) | OrderController | DeleteConfirmed | Redirect | Confirms order deletion. |
| `/orders/history` | OrderController | Index | Views/Order/Index.cshtml | Custom semantic route for order list. |
| `/orders/history/create` | OrderController | Create | Views/Order/Create.cshtml | Custom semantic route for order creation form. |
| `/orders/history/create` (POST) | OrderController | Create | Redirect | Submits new order data. |
| `/orders/{id:int}/summary` | OrderController | Details | Views/Order/Details.cshtml | Custom semantic route for order details. |
| `/orders/{id:int}/edit` | OrderController | Edit | Views/Order/Edit.cshtml | Custom semantic route for order edit form. |
| `/orders/{id:int}/edit` (POST) | OrderController | Edit | Redirect | Submits order updates. |
| `/orders/{id:int}/delete` | OrderController | Delete | Views/Order/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/orders/{id:int}/delete` (POST) | OrderController | DeleteConfirmed | Redirect | Confirms order deletion. |
| `/orders/search` | OrderController | Search | Views/Order/_OrderTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## InventoryItem Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/InventoryItem` | InventoryItemController | Index | Views/InventoryItem/Index.cshtml | Default MVC route for inventory item list. |
| `/InventoryItem/Index` | InventoryItemController | Index | Views/InventoryItem/Index.cshtml | Default MVC route for inventory item list. |
| `/InventoryItem/Details/{id}` | InventoryItemController | Details | Views/InventoryItem/Details.cshtml | Default MVC route for inventory item details. |
| `/InventoryItem/Create` | InventoryItemController | Create | Views/InventoryItem/Create.cshtml | Default MVC route for inventory item creation form. |
| `/InventoryItem/Create` (POST) | InventoryItemController | Create | Redirect | Submits new inventory item data. |
| `/InventoryItem/Edit/{id}` | InventoryItemController | Edit | Views/InventoryItem/Edit.cshtml | Default MVC route for inventory item edit form. |
| `/InventoryItem/Edit/{id}` (POST) | InventoryItemController | Edit | Redirect | Submits inventory item updates. |
| `/InventoryItem/Delete/{id}` | InventoryItemController | Delete | Views/InventoryItem/Delete.cshtml | Default MVC route for delete confirmation. |
| `/InventoryItem/Delete/{id}` (POST) | InventoryItemController | DeleteConfirmed | Redirect | Confirms inventory item deletion. |
| `/inventory` | InventoryItemController | Index | Views/InventoryItem/Index.cshtml | Custom semantic route for inventory list. |
| `/inventory/{id:int}` | InventoryItemController | Details | Views/InventoryItem/Details.cshtml | Custom semantic route for inventory item details. |
| `/inventory/create` | InventoryItemController | Create | Views/InventoryItem/Create.cshtml | Custom semantic route for inventory item creation form. |
| `/inventory/create` (POST) | InventoryItemController | Create | Redirect | Submits new inventory item data. |
| `/inventory/{id:int}/edit` | InventoryItemController | Edit | Views/InventoryItem/Edit.cshtml | Custom semantic route for inventory item edit form. |
| `/inventory/{id:int}/edit` (POST) | InventoryItemController | Edit | Redirect | Submits inventory item updates. |
| `/inventory/{id:int}/delete` | InventoryItemController | Delete | Views/InventoryItem/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/inventory/{id:int}/delete` (POST) | InventoryItemController | DeleteConfirmed | Redirect | Confirms inventory item deletion. |
| `/inventory/search` | InventoryItemController | Search | Views/InventoryItem/_InventoryItemTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## OrderItem Routes

| URL | Controller | Action | View/Partial | Notes |
|---|---|---|---|---|
| `/OrderItem` | OrderItemController | Index | Views/OrderItem/Index.cshtml | Default MVC route for order item list. |
| `/OrderItem/Index` | OrderItemController | Index | Views/OrderItem/Index.cshtml | Default MVC route for order item list. |
| `/OrderItem/Details/{id}` | OrderItemController | Details | Views/OrderItem/Details.cshtml | Default MVC route for order item details. |
| `/OrderItem/Create` | OrderItemController | Create | Views/OrderItem/Create.cshtml | Default MVC route for order item creation form. |
| `/OrderItem/Create` (POST) | OrderItemController | Create | Redirect | Submits new order item data. |
| `/OrderItem/Edit/{id}` | OrderItemController | Edit | Views/OrderItem/Edit.cshtml | Default MVC route for order item edit form. |
| `/OrderItem/Edit/{id}` (POST) | OrderItemController | Edit | Redirect | Submits order item updates. |
| `/OrderItem/Delete/{id}` | OrderItemController | Delete | Views/OrderItem/Delete.cshtml | Default MVC route for delete confirmation. |
| `/OrderItem/Delete/{id}` (POST) | OrderItemController | DeleteConfirmed | Redirect | Confirms order item deletion. |
| `/order-items` | OrderItemController | Index | Views/OrderItem/Index.cshtml | Custom semantic route for order item list. |
| `/order-items/{id:int}` | OrderItemController | Details | Views/OrderItem/Details.cshtml | Custom semantic route for order item details. |
| `/order-items/create` | OrderItemController | Create | Views/OrderItem/Create.cshtml | Custom semantic route for order item creation form. |
| `/order-items/create` (POST) | OrderItemController | Create | Redirect | Submits new order item data. |
| `/order-items/{id:int}/edit` | OrderItemController | Edit | Views/OrderItem/Edit.cshtml | Custom semantic route for order item edit form. |
| `/order-items/{id:int}/edit` (POST) | OrderItemController | Edit | Redirect | Submits order item updates. |
| `/order-items/{id:int}/delete` | OrderItemController | Delete | Views/OrderItem/Delete.cshtml | Custom semantic route for delete confirmation. |
| `/order-items/{id:int}/delete` (POST) | OrderItemController | DeleteConfirmed | Redirect | Confirms order item deletion. |
| `/order-items/search` | OrderItemController | Search | Views/OrderItem/_OrderItemTableRows.cshtml | **AJAX endpoint** - Returns table rows matching search term. |

---

## Lookup Routes (AJAX Autocomplete)

| URL | Controller | Action | Response | Notes |
|---|---|---|---|---|
| `/lookup/categories` | LookupController | Categories | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |
| `/lookup/suppliers` | LookupController | Suppliers | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |
| `/lookup/users` | LookupController | Users | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |
| `/lookup/products` | LookupController | Products | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |
| `/lookup/warehouses` | LookupController | Warehouses | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |
| `/lookup/orders` | LookupController | Orders | JSON | **AJAX endpoint** - Returns `[{id: int, text: string}]` for autocomplete. |

---

## Custom Routing Summary

### Lab 3: Read-Only Semantic Routes

| Custom URL | Controller | Action | Purpose |
|---|---|---|---|
| `/catalog` | ProductController | Index | Semantic product catalog page. |
| `/catalog/{id:int}` | ProductController | Details | Product details with integer route constraint. |
| `/vendors` | SupplierController | Index | Semantic supplier directory page. |
| `/vendors/{id:int}` | SupplierController | Details | Supplier details with integer route constraint. |
| `/storage` | WarehouseController | Index | Semantic warehouse overview page. |
| `/storage/{id:int}` | WarehouseController | Details | Warehouse details with integer route constraint. |
| `/orders/history` | OrderController | Index | Semantic order history page. |
| `/orders/{id:int}/summary` | OrderController | Details | Order details with integer route constraint. |

### Lab 4: Full CRUD Semantic Routes

| Custom URL | Controller | Action | Purpose |
|---|---|---|---|
| `/catalog/create` | ProductController | Create | Product creation form. |
| `/catalog/{id:int}/edit` | ProductController | Edit | Product update form with integer route constraint. |
| `/catalog/{id:int}/delete` | ProductController | Delete | Product deletion confirmation with integer route constraint. |
| `/catalog/search` | ProductController | Search | **AJAX** - Product search endpoint. |
| `/users` | UserController | Index | Semantic user list page. |
| `/users/{id:int}` | UserController | Details | User details with integer route constraint. |
| `/users/create` | UserController | Create | User creation form. |
| `/users/{id:int}/edit` | UserController | Edit | User update form with integer route constraint. |
| `/users/{id:int}/delete` | UserController | Delete | User deletion confirmation with integer route constraint. |
| `/users/search` | UserController | Search | **AJAX** - User search endpoint. |
| `/vendors/create` | SupplierController | Create | Supplier creation form. |
| `/vendors/{id:int}/edit` | SupplierController | Edit | Supplier update form with integer route constraint. |
| `/vendors/{id:int}/delete` | SupplierController | Delete | Supplier deletion confirmation with integer route constraint. |
| `/vendors/search` | SupplierController | Search | **AJAX** - Supplier search endpoint. |
| `/storage/create` | WarehouseController | Create | Warehouse creation form. |
| `/storage/{id:int}/edit` | WarehouseController | Edit | Warehouse update form with integer route constraint. |
| `/storage/{id:int}/delete` | WarehouseController | Delete | Warehouse deletion confirmation with integer route constraint. |
| `/storage/search` | WarehouseController | Search | **AJAX** - Warehouse search endpoint. |
| `/orders/history/create` | OrderController | Create | Order creation form. |
| `/orders/{id:int}/edit` | OrderController | Edit | Order update form with integer route constraint. |
| `/orders/{id:int}/delete` | OrderController | Delete | Order deletion confirmation with integer route constraint. |
| `/orders/search` | OrderController | Search | **AJAX** - Order search endpoint. |
| `/inventory` | InventoryItemController | Index | Semantic inventory list page. |
| `/inventory/{id:int}` | InventoryItemController | Details | Inventory item details with integer route constraint. |
| `/inventory/create` | InventoryItemController | Create | Inventory item creation form. |
| `/inventory/{id:int}/edit` | InventoryItemController | Edit | Inventory item update form with integer route constraint. |
| `/inventory/{id:int}/delete` | InventoryItemController | Delete | Inventory item deletion confirmation with integer route constraint. |
| `/inventory/search` | InventoryItemController | Search | **AJAX** - Inventory search endpoint. |
| `/order-items` | OrderItemController | Index | Semantic order item list page. |
| `/order-items/{id:int}` | OrderItemController | Details | Order item details with integer route constraint. |
| `/order-items/create` | OrderItemController | Create | Order item creation form. |
| `/order-items/{id:int}/edit` | OrderItemController | Edit | Order item update form with integer route constraint. |
| `/order-items/{id:int}/delete` | OrderItemController | Delete | Order item deletion confirmation with integer route constraint. |
| `/order-items/search` | OrderItemController | Search | **AJAX** - Order item search endpoint. |
| `/lookup/categories` | LookupController | Categories | **AJAX** - Category autocomplete endpoint. |
| `/lookup/suppliers` | LookupController | Suppliers | **AJAX** - Supplier autocomplete endpoint. |
| `/lookup/users` | LookupController | Users | **AJAX** - User autocomplete endpoint. |
| `/lookup/products` | LookupController | Products | **AJAX** - Product autocomplete endpoint. |
| `/lookup/warehouses` | LookupController | Warehouses | **AJAX** - Warehouse autocomplete endpoint. |
| `/lookup/orders` | LookupController | Orders | **AJAX** - Order autocomplete endpoint. |

---

## Routing Explanation

### Default MVC Route

The default MVC route still allows URLs like:

`/Product/Details/1`
`/Product/Create`
`/Product/Edit/1`
`/Product/Delete/1`

This default route serves as a fallback and is defined in `Program.cs`.

### Lab 3: Semantic Routes

Lab 3 added semantic routes for improved readability:

`/catalog/1` (instead of `/Product/Details/1`)
`/vendors` (instead of `/Supplier/Index`)
`/storage/5` (instead of `/Warehouse/Details/5`)
`/orders/history` (instead of `/Order/Index`)

### Lab 4: Extended CRUD and AJAX Routes

Lab 4 extended semantic routes to include full CRUD operations:

`/catalog/create` (Create form)
`/catalog/1/edit` (Edit form)
`/catalog/1/delete` (Delete confirmation)
`/catalog/search` (AJAX search endpoint)

All CRUD actions are available through both semantic and default MVC routes.

### Route Constraints

The `{id:int}` part is a route constraint.  
It means that the route only matches if the `id` value is an integer.

Example:

- `/catalog/1` is valid.
- `/catalog/test` is not valid for the route `/catalog/{id:int}`.

---

## Notes

### Architecture

The application uses attribute routing for the custom semantic URLs.  
The default route in `Program.cs` remains available as a fallback.

### Validation

- **Server-side**: DataAnnotation attributes (`[Required]`, `[StringLength]`, etc.) with ModelState validation
- **Client-side**: jQuery Validation framework for real-time user feedback
- **Form Models**: ViewModels with validation attributes are used for all CRUD operations

### Lab 4 Features

- **Form Models**: All entities use ViewModels with DataAnnotation validation
- **Repository Pattern**: All repositories implement CRUD + Search methods with Include() chains
- **Delete Safety**: Dependencies are checked before deletion; operations fail gracefully with user feedback
- **Autocomplete Dropdowns**: Reusable component for selecting related entities (Product/Category/Supplier/Warehouse/User/Order)
- **Custom Date Input**: Language-aware display (Croatian dd.MM.yyyy. / English MM/dd/yyyy) with ISO storage format
- **AJAX Search**: 300ms debounce, fade animations, partial view returns for dynamic table updates
- **Toast Notifications**: TempData-based success messages after successful CRUD operations
- **Delete Confirmations**: JavaScript confirmation dialogs with customizable messages (ui-polish.js)
- **Row Highlighting**: URL parameter `?highlightId={id}` highlights and scrolls to specific table row
- **Page Load Animation**: `js-enhanced` class added to body for CSS animation hooks