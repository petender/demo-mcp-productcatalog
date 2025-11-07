using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using ProductCatalogMCPServer.Services;

namespace ProductCatalogMCPServer;

/// <summary>
/// Provides product inventory management tools for the MCP server.
/// Loads product data from a JSON file on startup and maintains it in memory.
/// All modifications are temporary and reset when the server restarts.
/// </summary>
[McpServerToolType]
internal class ProductCatalogTools
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductCatalogTools> _logger;

    public ProductCatalogTools(
        IProductService productService,
        ILogger<ProductCatalogTools> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [McpServerTool]
    [Description("Provides the whole list of products in the inventory")]
    public async Task<ProductCollection> ListProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return new ProductCollection 
        { 
            Products = products 
        };
    }

    [McpServerTool]
    [Description("Adds a new product to the inventory")]
    public async Task<string> AddProduct(
        [Description("Name of the product")] string name,
        [Description("Short description of the product")] string description,
        [Description("EAN barcode of the product")] string ean,
        [Description("Cost/price of the product")] decimal cost,
        [Description("Number of units in stock")] int unitsInStock,
        [Description("Brand/manufacturer of the product")] string brand = "",
        [Description("Comma-separated list of product categories")] string categories = "")
    {
        var product = new Product
        {
            Name = name?.Trim() ?? string.Empty,
            Description = description?.Trim() ?? string.Empty,
            EAN = ean?.Trim() ?? string.Empty,
            Cost = cost,
            UnitsInStock = unitsInStock,
            Brand = brand?.Trim() ?? string.Empty,
            Categories = ParseCommaSeparatedString(categories)
        };

        var success = await _productService.AddProductAsync(product);
        
        if (!success)
        {
            return $"Product with EAN '{product.EAN}' already exists.";
        }
        
        return $"Successfully added product: {product.FullDisplayName} (EAN: {product.EAN})";
    }

    [McpServerTool]
    [Description("Updates an existing product by EAN")]
    public async Task<string> UpdateProduct(
        [Description("EAN barcode of the product to update")] string ean,
        [Description("New product name (optional)")] string? name = null,
        [Description("New product description (optional)")] string? description = null,
        [Description("New cost/price (optional)")] decimal? cost = null,
        [Description("New brand/manufacturer (optional)")] string? brand = null,
        [Description("New comma-separated list of categories (optional)")] string? categories = null)
    {
        var success = await _productService.UpdateProductAsync(ean, product =>
        {
            // Update fields if provided
            if (!string.IsNullOrWhiteSpace(name))
                product.Name = name.Trim();
            
            if (!string.IsNullOrWhiteSpace(description))
                product.Description = description.Trim();
            
            if (cost.HasValue && cost.Value >= 0)
                product.Cost = cost.Value;
            
            if (!string.IsNullOrWhiteSpace(brand))
                product.Brand = brand.Trim();
            
            if (categories != null)
                product.Categories = ParseCommaSeparatedString(categories);
        });

        if (!success)
        {
            return $"Product with EAN '{ean}' not found.";
        }
        
        return $"Successfully updated product with EAN: {ean}";
    }

    [McpServerTool]
    [Description("Updates the stock quantity for a product")]
    public async Task<string> UpdateStock(
        [Description("EAN barcode of the product")] string ean,
        [Description("New stock quantity")] int newQuantity)
    {
        if (newQuantity < 0)
        {
            return "Stock quantity cannot be negative.";
        }

        var success = await _productService.UpdateStockAsync(ean, newQuantity);

        if (!success)
        {
            return $"Product with EAN '{ean}' not found.";
        }
        
        return $"Successfully updated stock for product EAN {ean} to {newQuantity} units.";
    }

    [McpServerTool]
    [Description("Removes a product by EAN")]
    public async Task<string> RemoveProduct(
        [Description("EAN barcode of the product to remove")] string ean)
    {
        var success = await _productService.RemoveProductAsync(ean);

        if (!success)
        {
            return $"Product with EAN '{ean}' not found.";
        }
        
        return $"Successfully removed product with EAN: {ean}";
    }

    [McpServerTool]
    [Description("Searches for products by name, description, EAN, brand, or categories")]
    public async Task<ProductCollection> SearchProducts(
        [Description("Search term to find in product data")] string searchTerm)
    {
        var matchingProducts = await _productService.SearchProductsAsync(searchTerm);
        
        return new ProductCollection 
        { 
            Products = matchingProducts 
        };
    }

    [McpServerTool]
    [Description("Gets products with low stock (below specified threshold)")]
    public async Task<ProductCollection> GetLowStockProducts(
        [Description("Stock threshold (default: 10)")] int threshold = 10)
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var lowStockProducts = allProducts.Where(p => p.UnitsInStock <= threshold).ToList();
        
        return new ProductCollection 
        { 
            Products = lowStockProducts 
        };
    }

    // Private helper methods
    private static List<string> ParseCommaSeparatedString(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                   .Select(s => s.Trim())
                   .Where(s => !string.IsNullOrEmpty(s))
                   .ToList();
    }
}