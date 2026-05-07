# Sitemap and Routing Model

## Project

This document describes the available URLs in the InventoryManagement ASP.NET Core MVC application.

The application uses the default MVC route as a fallback:

`/{controller}/{action}/{id?}`

In Lab 3, additional semantic custom routes were added using attribute routing.  
The default route is still available, but custom routes are used to make selected URLs more meaningful and business-oriented.

---

## Home Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/` | HomeController | Index | Views/Home/Index.cshtml | Main dashboard page. |
| `/Home/Index` | HomeController | Index | Views/Home/Index.cshtml | Default MVC route. |
| `/Home/Privacy` | HomeController | Privacy | Views/Home/Privacy.cshtml | Standard privacy page. |
| `/Home/Error` | HomeController | Error | Views/Shared/Error.cshtml | Error page. |

---

## Product Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/Product` | ProductController | Index | Views/Product/Index.cshtml | Default MVC route for product list. |
| `/Product/Index` | ProductController | Index | Views/Product/Index.cshtml | Default MVC route for product list. |
| `/Product/Details/{id}` | ProductController | Details | Views/Product/Details.cshtml | Default MVC route for product details. |
| `/catalog` | ProductController | Index | Views/Product/Index.cshtml | Custom semantic route. |
| `/catalog/{id:int}` | ProductController | Details | Views/Product/Details.cshtml | Custom semantic route with integer route constraint. |

---

## Supplier Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/Supplier` | SupplierController | Index | Views/Supplier/Index.cshtml | Default MVC route for supplier list. |
| `/Supplier/Index` | SupplierController | Index | Views/Supplier/Index.cshtml | Default MVC route for supplier list. |
| `/Supplier/Details/{id}` | SupplierController | Details | Views/Supplier/Details.cshtml | Default MVC route for supplier details. |
| `/vendors` | SupplierController | Index | Views/Supplier/Index.cshtml | Custom semantic route. |
| `/vendors/{id:int}` | SupplierController | Details | Views/Supplier/Details.cshtml | Custom semantic route with integer route constraint. |

---

## Category Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/Category` | CategoryController | Index | Views/Category/Index.cshtml | Default MVC route for category list. |
| `/Category/Index` | CategoryController | Index | Views/Category/Index.cshtml | Default MVC route for category list. |
| `/Category/Details/{id}` | CategoryController | Details | Views/Category/Details.cshtml | Default MVC route for category details. |

---

## Warehouse Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/Warehouse` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Default MVC route for warehouse list. |
| `/Warehouse/Index` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Default MVC route for warehouse list. |
| `/Warehouse/Details/{id}` | WarehouseController | Details | Views/Warehouse/Details.cshtml | Default MVC route for warehouse details. |
| `/storage` | WarehouseController | Index | Views/Warehouse/Index.cshtml | Custom semantic route. |
| `/storage/{id:int}` | WarehouseController | Details | Views/Warehouse/Details.cshtml | Custom semantic route with integer route constraint. |

---

## User Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/User` | UserController | Index | Views/User/Index.cshtml | Default MVC route for user list. |
| `/User/Index` | UserController | Index | Views/User/Index.cshtml | Default MVC route for user list. |
| `/User/Details/{id}` | UserController | Details | Views/User/Details.cshtml | Default MVC route for user details. |

---

## Order Routes

| URL | Controller | Action | View | Notes |
|---|---|---|---|---|
| `/Order` | OrderController | Index | Views/Order/Index.cshtml | Default MVC route for order list. |
| `/Order/Index` | OrderController | Index | Views/Order/Index.cshtml | Default MVC route for order list. |
| `/Order/Details/{id}` | OrderController | Details | Views/Order/Details.cshtml | Default MVC route for order details. |
| `/orders/history` | OrderController | Index | Views/Order/Index.cshtml | Custom semantic route. |
| `/orders/{id:int}/summary` | OrderController | Details | Views/Order/Details.cshtml | Custom semantic route with integer route constraint. |

---

## Custom Routing Summary

The following custom routes were added for Lab 3:

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

---

## Routing Explanation

The default MVC route still allows URLs like:

`/Product/Details/1`

However, Lab 3 adds semantic routes such as:

`/catalog/1`

Both routes can point to the same controller action, but the semantic route is easier to understand from a user perspective.

The `{id:int}` part is a route constraint.  
It means that the route only matches if the `id` value is an integer.

Example:

- `/catalog/1` is valid.
- `/catalog/test` is not valid for the route `/catalog/{id:int}`.

---

## Notes

The application uses attribute routing for the custom semantic URLs.  
The default route in `Program.cs` remains available as a fallback.