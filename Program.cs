using ModelContextProtocol.Server;
using Microsoft.Extensions.DependencyInjection;
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
        // Log environment information for diagnostics
        Console.WriteLine($"[DIAGNOSTIC] AppContext.BaseDirectory: {AppContext.BaseDirectory}");
        Console.WriteLine($"[DIAGNOSTIC] Environment.CurrentDirectory: {Environment.CurrentDirectory}");
        Console.WriteLine($"[DIAGNOSTIC] ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
        
        var productCatalogConfig = configuration.GetSection(ProductCatalogMCPServerConfiguration.SectionName).Get<ProductCatalogMCPServerConfiguration>();
        
        if (productCatalogConfig == null || string.IsNullOrEmpty(productCatalogConfig.ProductsPath))
        {
            Console.WriteLine("[ERROR] Product catalog configuration or ProductsPath not found. Using empty product list.");
            return new List<Product>();
        }

        Console.WriteLine($"[DIAGNOSTIC] Configured ProductsPath: {productCatalogConfig.ProductsPath}");

        var productsPath = Path.Combine(AppContext.BaseDirectory, productCatalogConfig.ProductsPath);
        Console.WriteLine($"[DIAGNOSTIC] Resolved full path: {productsPath}");
        Console.WriteLine($"[DIAGNOSTIC] File exists: {File.Exists(productsPath)}");

        // Also try listing the Data directory to verify its contents
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        if (Directory.Exists(dataDir))
        {
            Console.WriteLine($"[DIAGNOSTIC] Data directory contents:");
            foreach (var file in Directory.GetFiles(dataDir))
            {
                Console.WriteLine($"  - {file}");
            }
        }
        else
        {
            Console.WriteLine($"[DIAGNOSTIC] Data directory does not exist at: {dataDir}");
        }

        if (!File.Exists(productsPath))
        {
            Console.WriteLine($"[ERROR] Products file not found at: {productsPath}. Using empty product list.");
            return new List<Product>();
        }

        var jsonContent = await File.ReadAllTextAsync(productsPath);
        Console.WriteLine($"[DIAGNOSTIC] JSON content length: {jsonContent?.Length ?? 0} bytes");
        
        var products = JsonSerializer.Deserialize<List<Product>>(jsonContent, GetJsonOptions());

        Console.WriteLine($"[SUCCESS] Loaded {products?.Count ?? 0} products from file: {productsPath}");
        return products ?? new List<Product>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Error loading products from file: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
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