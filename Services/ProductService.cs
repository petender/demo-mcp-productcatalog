using System.Text.Json;
using Microsoft.Extensions.Options;

namespace ProductCatalogMCPServer.Services;

/// <summary>
/// Service for managing product data in memory
/// </summary>
public class ProductService : IProductService
{
    private readonly List<Product> _products;
    private readonly object _productsLock = new();
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        List<Product> products,
        ILogger<ProductService> logger)
    {
        _products = products ?? throw new ArgumentNullException(nameof(products));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<List<Product>> GetAllProductsAsync()
    {
        lock (_productsLock)
        {
            return Task.FromResult(new List<Product>(_products));
        }
    }

    public Task<bool> AddProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        lock (_productsLock)
        {
            // Check if product with same EAN already exists
            if (_products.Any(p => string.Equals(p.EAN, product.EAN, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.FromResult(false);
            }

            _products.Add(product);
            _logger.LogInformation("Added new product: {FullDisplayName} (EAN: {EAN})", product.FullDisplayName, product.EAN);
            return Task.FromResult(true);
        }
    }

    public Task<bool> UpdateProductAsync(string ean, Action<Product> updateAction)
    {
        if (string.IsNullOrWhiteSpace(ean))
            throw new ArgumentException("EAN cannot be null or empty", nameof(ean));
        
        if (updateAction == null)
            throw new ArgumentNullException(nameof(updateAction));

        lock (_productsLock)
        {
            var product = _products.FirstOrDefault(p => 
                string.Equals(p.EAN, ean, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                return Task.FromResult(false);
            }

            updateAction(product);
            _logger.LogInformation("Updated product with EAN: {EAN}", ean);
            return Task.FromResult(true);
        }
    }

    public Task<bool> RemoveProductAsync(string ean)
    {
        if (string.IsNullOrWhiteSpace(ean))
            throw new ArgumentException("EAN cannot be null or empty", nameof(ean));

        lock (_productsLock)
        {
            var product = _products.FirstOrDefault(p => 
                string.Equals(p.EAN, ean, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                return Task.FromResult(false);
            }

            _products.Remove(product);
            _logger.LogInformation("Removed product with EAN: {EAN}", ean);
            return Task.FromResult(true);
        }
    }

    public Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAllProductsAsync();
        }

        var searchTermLower = searchTerm.Trim().ToLowerInvariant();

        lock (_productsLock)
        {
            var matchingProducts = _products.Where(p =>
                p.Name.ToLowerInvariant().Contains(searchTermLower) ||
                p.Description.ToLowerInvariant().Contains(searchTermLower) ||
                p.EAN.ToLowerInvariant().Contains(searchTermLower) ||
                p.Brand.ToLowerInvariant().Contains(searchTermLower) ||
                p.Categories.Any(category => category.ToLowerInvariant().Contains(searchTermLower))
            ).ToList();

            return Task.FromResult(matchingProducts);
        }
    }

    public Task<bool> UpdateStockAsync(string ean, int newQuantity)
    {
        if (string.IsNullOrWhiteSpace(ean))
            throw new ArgumentException("EAN cannot be null or empty", nameof(ean));

        if (newQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));

        lock (_productsLock)
        {
            var product = _products.FirstOrDefault(p => 
                string.Equals(p.EAN, ean, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                return Task.FromResult(false);
            }

            var oldQuantity = product.UnitsInStock;
            product.UnitsInStock = newQuantity;
            _logger.LogInformation("Updated stock for product {EAN}: {OldQuantity} -> {NewQuantity}", 
                ean, oldQuantity, newQuantity);
            return Task.FromResult(true);
        }
    }
}
