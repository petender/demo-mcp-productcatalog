using System.Text.Json.Serialization;

namespace ProductCatalogMCPServer;

/// <summary>
/// Represents a product in the retail inventory system
/// </summary>
public class Product
{
    /// <summary>
    /// The product's name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short description of the product
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product's EAN barcode
    /// </summary>
    [JsonPropertyName("ean")]
    public string EAN { get; set; } = string.Empty;

    /// <summary>
    /// The cost/price of the product
    /// </summary>
    [JsonPropertyName("cost")]
    public decimal Cost { get; set; }

    /// <summary>
    /// Number of units currently in stock
    /// </summary>
    [JsonPropertyName("units_in_stock")]
    public int UnitsInStock { get; set; }

    /// <summary>
    /// List of product categories/tags
    /// </summary>
    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = new();

    /// <summary>
    /// Product brand/manufacturer
    /// </summary>
    [JsonPropertyName("brand")]
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Gets the product's full display name with brand
    /// </summary>
    public string FullDisplayName => !string.IsNullOrEmpty(Brand) ? $"{Brand} {Name}" : Name;
}

/// <summary>
/// Container for a collection of products
/// </summary>
public class ProductCollection
{
    /// <summary>
    /// List of products
    /// </summary>
    public List<Product> Products { get; set; } = new();
}

