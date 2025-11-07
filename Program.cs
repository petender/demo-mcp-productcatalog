using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProductCatalogMCPServer;
using ProductCatalogMCPServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure the Product Catalog MCP Server settings
builder.Services.Configure<ProductCatalogMCPServerConfiguration>(
    builder.Configuration.GetSection(ProductCatalogMCPServerConfiguration.SectionName));

// Load products data and register as singleton
var productsData = await LoadProductsAsync(builder.Configuration);
builder.Services.AddSingleton(productsData);

// Register the product service
builder.Services.AddScoped<IProductService, ProductService>();

// Add the MCP services: the transport to use (HTTP) and the tools to register.
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();
    
var app = builder.Build();

// Configure the application to use the MCP server
app.MapMcp();

// Run the application
// This will start the MCP server and listen for incoming requests.
app.Run();

// Helper method to load products from JSON file
static async Task<List<Product>> LoadProductsAsync(IConfiguration configuration)
{
    try
    {
        var productCatalogConfig = configuration.GetSection(ProductCatalogMCPServerConfiguration.SectionName).Get<ProductCatalogMCPServerConfiguration>();
        
        if (productCatalogConfig == null || string.IsNullOrEmpty(productCatalogConfig.ProductsPath))
        {
            Console.WriteLine("Product catalog configuration or ProductsPath not found. Using empty product list.");
            return new List<Product>();
        }

        if (!File.Exists(productCatalogConfig.ProductsPath))
        {
            Console.WriteLine($"Products file not found at: {productCatalogConfig.ProductsPath}. Using empty product list.");
            return new List<Product>();
        }

        var jsonContent = await File.ReadAllTextAsync(productCatalogConfig.ProductsPath);
        var products = JsonSerializer.Deserialize<List<Product>>(jsonContent, GetJsonOptions());

        Console.WriteLine($"Loaded {products?.Count ?? 0} products from file: {productCatalogConfig.ProductsPath}");
        return products ?? new List<Product>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading products from file: {ex.Message}. Using empty product list.");
        return new List<Product>();
    }
}

// Helper method for JSON serialization options
static JsonSerializerOptions GetJsonOptions()
{
    return new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}