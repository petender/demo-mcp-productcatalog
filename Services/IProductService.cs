namespace ProductCatalogMCPServer.Services;

/// <summary>
/// Interface for managing product data operations
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Gets all products
    /// </summary>
    /// <returns>A list of all products</returns>
    Task<List<Product>> GetAllProductsAsync();

    /// <summary>
    /// Adds a new product to the collection
    /// </summary>
    /// <param name="product">The product to add</param>
    /// <returns>True if added successfully, false if product with same EAN already exists</returns>
    Task<bool> AddProductAsync(Product product);

    /// <summary>
    /// Updates an existing product by EAN
    /// </summary>
    /// <param name="ean">EAN of the product to update</param>
    /// <param name="updateAction">Action to perform the update</param>
    /// <returns>True if product was found and updated, false otherwise</returns>
    Task<bool> UpdateProductAsync(string ean, Action<Product> updateAction);

    /// <summary>
    /// Removes a product by EAN
    /// </summary>
    /// <param name="ean">EAN of the product to remove</param>
    /// <returns>True if product was found and removed, false otherwise</returns>
    Task<bool> RemoveProductAsync(string ean);

    /// <summary>
    /// Searches for products based on a search term
    /// </summary>
    /// <param name="searchTerm">The term to search for</param>
    /// <returns>A list of matching products</returns>
    Task<List<Product>> SearchProductsAsync(string searchTerm);

    /// <summary>
    /// Updates the stock quantity for a product
    /// </summary>
    /// <param name="ean">EAN of the product</param>
    /// <param name="newQuantity">New stock quantity</param>
    /// <returns>True if product was found and updated, false otherwise</returns>
    Task<bool> UpdateStockAsync(string ean, int newQuantity);
}
