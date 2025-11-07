# Product Catalog MCP Server

This is a Model Context Protocol (MCP) server for managing a retail product inventory system. The server provides tools for managing products including adding, updating, removing, and searching for products in the catalog.

## Features

- **Product Management**: Add, update, and remove products from the inventory
- **Search Functionality**: Search products by name, description, EAN, brand, or categories
- **Stock Management**: Track and update product stock levels
- **Low Stock Alerts**: Identify products with low inventory
- **JSON Data Storage**: Products are stored in a JSON file for easy management

## Product Data Model

Each product contains the following information:
- **Name**: Product name
- **Description**: Short product description
- **EAN**: European Article Number (barcode)
- **Cost**: Product price/cost
- **Units in Stock**: Current inventory count
- **Brand**: Product manufacturer/brand
- **Categories**: List of product categories/tags

## Available MCP Tools

### ListProducts
Returns the complete list of all products in the inventory.

### AddProduct
Adds a new product to the inventory. Requires:
- name (string): Product name
- description (string): Product description
- ean (string): EAN barcode
- cost (decimal): Product cost
- unitsInStock (int): Stock quantity
- brand (string, optional): Product brand
- categories (string, optional): Comma-separated categories

### UpdateProduct
Updates an existing product by EAN. Optional parameters:
- name, description, cost, brand, categories

### UpdateStock
Updates the stock quantity for a specific product by EAN.

### RemoveProduct
Removes a product from the inventory by EAN.

### SearchProducts
Searches for products using a search term that matches against name, description, EAN, brand, or categories.

### GetLowStockProducts
Returns products with stock levels below a specified threshold (default: 10 units).

## Configuration

The server uses `ProductCatalogMCPServer` configuration section in `appsettings.Development.json`:

```json
{
  "ProductCatalogMCPServer": {
    "ProductsPath": ".\\Data\\products.json"
  }
}
```

## Running the Server

1. Build the project: `dotnet build`
2. Run the server: `dotnet run`
3. The server will listen on `http://localhost:47002`

The server loads product data from the JSON file on startup and maintains it in memory. All modifications are temporary and will reset when the server restarts.

## Sample Data

The server comes with sample product data including electronics, appliances, clothing, and office supplies to demonstrate the functionality.