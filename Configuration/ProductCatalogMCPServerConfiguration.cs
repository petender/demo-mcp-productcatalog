namespace ProductCatalogMCPServer;

/// <summary>
/// Configuration settings for the Product Catalog MCP Server
/// </summary>
public class ProductCatalogMCPServerConfiguration
{
    public const string SectionName = "ProductCatalogMCPServer";
    
    /// <summary>
    /// The path of the products file for Product Catalog MCP Server
    /// </summary>
    public string ProductsPath { get; set; } = string.Empty;
}
